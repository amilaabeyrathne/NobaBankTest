using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Factories;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CarRentalSystem.Application.Services;

public class ReservationService : IReservationService
{
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly PricingStrategyFactory _pricingStrategyFactory;
    private readonly ICarCategoryRepository _carCategoryRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        ICarRepository carRepository,
        IReservationRepository reservationRepository,
        PricingStrategyFactory pricingStrategyFactory,
        ICarCategoryRepository carCategoryRepository,
        ILogger<ReservationService> logger)
    {
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _pricingStrategyFactory = pricingStrategyFactory;
        _carCategoryRepository = carCategoryRepository;
        _logger = logger;
    }

    public async Task<ReservationResponseDto> CreateReservationAsync(ReservationDto reservationDto, CancellationToken cancellationToken = default)
    {
        if (reservationDto == null) throw new ArgumentNullException(nameof(reservationDto));

        var car = await _carRepository.GetByIdAsync(reservationDto.CarId, cancellationToken);
        if (car == null) throw new InvalidOperationException("Car not found.");
        if (!car.IsAvailableToRent) throw new InvalidOperationException("Car is not available to rent.");

        var reservation = new Reservation(
            reservationDto.CarId,
            reservationDto.CustomerSocialSecurityNumber,
            DateTime.UtcNow,
            reservationDto.PickupMeterReading);

        //using var transactionScope = new TransactionScope(); cannot use with Sqlite
        try
        {
            car.MarkAsUnavailable();

            await _reservationRepository.AddReservationAsync(reservation, cancellationToken);

            await _reservationRepository.SaveChangesAsync(cancellationToken);

           //transactionScope.Complete();
        }
        catch (Exception ex)
        {
            _logger.LogError("Reservation  failed for carId {carId}", car.Id);
            throw new InvalidOperationException("Car may not available to rent. Please try again later.", ex);
        }

        return new ReservationResponseDto
        {
            Id = reservation.Id
        };
       
    }

    public async Task<ReturnResponseDto> CreateReturnAsync(ReturnDto returnDto, CancellationToken cancellationToken = default)
    {
        if (returnDto == null) throw new ArgumentNullException(nameof(returnDto));

        var reservation = await _reservationRepository.GetByBookingIdAsync(returnDto.BookingNumber, cancellationToken);

        if (reservation == null) throw new InvalidOperationException("Reservation not found.");
        if (reservation.IsReturned) throw new InvalidOperationException("Reservation already returned.");

        var car = reservation.Car;
        if (car == null) throw new InvalidOperationException("Car not found for reservation.");

        var days = Math.Max(1, (int)Math.Ceiling((DateTime.UtcNow - reservation.PickupDateTime).TotalDays)); //Minimum charge is 1 day, and partial days are rounded up.
        var km = returnDto.ReturnMeterReading - reservation.PickupMeterReading;

        var category = await _carCategoryRepository.GetByIdAsync(car.CategoryId, cancellationToken)
                       ?? throw new InvalidOperationException("Car category not found.");

        var strategy = _pricingStrategyFactory.GetStrategy(category.PricingStrategy);
        var price = strategy.CalculatePrice(days, km, category.BaseDayRental, category.BaseKmPrice, category.DayMultiplier, category.KmMultiplier);

        try
        {
            reservation.RegisterReturn(DateTime.UtcNow, returnDto.ReturnMeterReading, price);

            car.MarkAsAvailable(returnDto.ReturnMeterReading);

            await _reservationRepository.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            _logger.LogError("Return failed for booking number {bookingNumber}", returnDto.BookingNumber);
            throw new InvalidOperationException("Some thing went wrong. Please try again later.", ex);
        }

        return new ReturnResponseDto
        {
            RentalAmount = reservation.CalculatedPrice ?? 0
        };
    }
}

