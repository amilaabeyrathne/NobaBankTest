using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Factory;
using CarRentalSystem.Domain.PricingStrategies;
using CarRentalSystem.Domain.Primitives;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using Xunit;

namespace CarRentalSystem.Tests;

public class ReservationServiceTests
{
    private readonly Mock<ICarRepository> _mockCarRepository;
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    private readonly Mock<ICarCategoryRepository> _mockCarCategoryRepository;
    private readonly Mock<IPricingStrategyFactory> _mockPricingStrategyFactory;
    private readonly ReservationService _reservationService;
    private readonly Mock<ILogger<ReservationService>> _mockLogger;

    public ReservationServiceTests()
    {
        _mockCarRepository = new Mock<ICarRepository>();
        _mockReservationRepository = new Mock<IReservationRepository>();
        _mockCarCategoryRepository = new Mock<ICarCategoryRepository>();
        _mockLogger = new Mock<ILogger<ReservationService>>();
        _mockPricingStrategyFactory = new Mock<IPricingStrategyFactory>(); ;
        _reservationService = new ReservationService(
            _mockCarRepository.Object,
            _mockReservationRepository.Object,
            _mockPricingStrategyFactory.Object,
            _mockCarCategoryRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldReturnBookingId()
    {
        var carId = Guid.NewGuid();
        var (car, reservationDto) = SetupCreateReservationMocks(carId);

        var result = await _reservationService.CreateReservationAsync(reservationDto);

        Assert.NotEqual(Guid.Empty, result.Id);
        _mockCarRepository.Verify(x => x.GetByIdAsync(carId, It.IsAny<CancellationToken>()), Times.Once);
        _mockReservationRepository.Verify(x => x.AddReservationAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockReservationRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldReturnBookingIdAndCarShouldNotBeAvailableToBook()
    {
        var carId = Guid.NewGuid();
        var (car, reservationDto) = SetupCreateReservationMocks(carId);

        var result = await _reservationService.CreateReservationAsync(reservationDto);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.False(car.IsAvailableToRent);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldBubbleWrappedException_WhenSaveChangesFails()
    {
        var carId = Guid.NewGuid();
        var (_, reservationDto) = SetupCreateReservationMocks(carId);

        _mockReservationRepository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReservationAsync(reservationDto));
        Assert.Contains("Car may not available to rent. Please try again later.", ex.Message);
    }

    private (Car car, ReservationDto reservationDto) SetupCreateReservationMocks(Guid carId)
    {

        var car = new Car("ABC123", 1, 1000, "Toyota", "Camry", "Blue", isAvailableToRent: true);
        var reservationDto = new ReservationDto
        {
            CarId = carId,
            CustomerSocialSecurityNumber = "1234567890",
            PickupMeterReading = 1000
        };

        _mockCarRepository
            .Setup(x => x.GetByIdAsync(carId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(car);

        _mockReservationRepository
            .Setup(x => x.AddReservationAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reservation r, CancellationToken ct) => r);

        _mockReservationRepository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return (car, reservationDto);
    }

    // calculating return amounts related tests can be added here

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForSmallCar_OneDay_ZeroKm()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 1; 
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;
        
        var category = new CarCategory("Small Car",CarPricingStrategy.SmallCar, 1.0m, 0m, true, baseKmPrice, baseDayRental);
        var car = new Car("ABC123", categoryId, pickupMeterReading, "Toyota", "Camry", "Blue", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        var strategy = new SmallCarPricingStrategy();
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id; 
        
        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = pickupMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        var result = await _reservationService.CreateReturnAsync(returnDto);

        var expectedPrice = baseDayRental * 1; 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForCombi_OneDay_ZeroKm()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 2; 
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;
        
        var category = new CarCategory("Combi", CarPricingStrategy.Combi, 1.3m, 1.0m, true, baseKmPrice, baseDayRental);
        var car = new Car("XYZ789", categoryId, pickupMeterReading, "Volvo", "V70", "Silver", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        var strategy = new CombiPricingStrategy();
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id; 
        
        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = pickupMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

      
        var result = await _reservationService.CreateReturnAsync(returnDto);

       
        var expectedPrice = (baseDayRental * 1 * 1.3m) + (baseKmPrice * 0); 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForTruck_OneDay_ZeroKm()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 3; 
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;
       
        var category = new CarCategory("Truck", CarPricingStrategy.Truck, 1.5m, 1.5m, true, baseKmPrice, baseDayRental);
        var car = new Car("TRUCK1", categoryId, pickupMeterReading, "Scania", "R500", "White", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        SetReservationCar(reservation, car);
        var strategy = new TruckPricingStrategy();
        var bookingId = reservation.Id; 
        
        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = pickupMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        var result = await _reservationService.CreateReturnAsync(returnDto);

        var expectedPrice = (baseDayRental * 1 * 1.5m) + (baseKmPrice * 0 * 1.5m); 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task ThrowException_ShouldCalculatePrice_ForTruck_NegativeBaseRental()
    {
        var baseDayRental = -100m;
        var baseKmPrice = 1m;
        var categoryId = 3;
        var carId = Guid.NewGuid();

        var pickupDateTime = DateTime.UtcNow.AddHours(-20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;

        var category = new CarCategory("Truck", CarPricingStrategy.Truck, 1.5m, 1.5m, true, baseKmPrice, baseDayRental);
        var car = new Car("TRUCK1", categoryId, pickupMeterReading, "Scania", "R500", "White", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        var strategy = new TruckPricingStrategy();
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id;

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = pickupMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReturnAsync(returnDto));
    }

    [Fact]
    public async Task ThrowException_ShouldCalculatePrice_ForTruck_NegativeReturnMeterReading()
    {
        var baseDayRental = -100m;
        var baseKmPrice = 1m;
        var categoryId = 3;
        var carId = Guid.NewGuid();

        var pickupDateTime = DateTime.UtcNow.AddHours(-20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;

        var category = new CarCategory("Truck", CarPricingStrategy.Truck, 1.5m, 1.5m, true, baseKmPrice, baseDayRental);
        var car = new Car("TRUCK1", categoryId, pickupMeterReading, "Scania", "R500", "White", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        var strategy = new TruckPricingStrategy();

        SetReservationCar(reservation, car);

        var bookingId = reservation.Id;

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = 890
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReturnAsync(returnDto));
    }

    [Fact]
    public async Task ThrowException_ShouldCalculatePrice_ForTruck_PastReturnDateThanPickUpDate()
    {
        var baseDayRental = -100m;
        var baseKmPrice = 1m;
        var categoryId = 3;
        var carId = Guid.NewGuid();

        var pickupDateTime = DateTime.UtcNow.AddHours(20);
        var returnDateTime = DateTime.UtcNow;
        var pickupMeterReading = 1000;

        var category = new CarCategory("Truck", CarPricingStrategy.Truck, 1.5m, 1.5m, true, baseKmPrice, baseDayRental);
        var car = new Car("TRUCK1", categoryId, pickupMeterReading, "Scania", "R500", "White", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        var strategy = new TruckPricingStrategy();

        SetReservationCar(reservation, car);

        var bookingId = reservation.Id;

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = 890
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReturnAsync(returnDto));
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForSmallCar_LessThanOneDay_WithKms()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 1;
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-5); 
        var pickupMeterReading = 1000;
        var returnMeterReading = 1500; 
        
        var category = new CarCategory("Small Car", CarPricingStrategy.SmallCar, 1.0m, 0m, true, baseKmPrice, baseDayRental);
        var car = new Car("ABC123", categoryId, pickupMeterReading, "Toyota", "Camry", "Blue", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id;
        var strategy = new SmallCarPricingStrategy();

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = returnMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        var result = await _reservationService.CreateReturnAsync(returnDto);

        var expectedPrice = baseDayRental * 1; 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForCombi_LessThanOneDay_WithKms()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 2;
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-5); 
        var pickupMeterReading = 1000;
        var returnMeterReading = 1500; 
        
        var category = new CarCategory("Combi", CarPricingStrategy.Combi, 1.3m, 1.0m, true, baseKmPrice, baseDayRental);
        var car = new Car("XYZ789", categoryId, pickupMeterReading, "Volvo", "V70", "Silver", isAvailableToRent: false);
        var reservation = new Reservation(car.Id, "1234567890", pickupDateTime, pickupMeterReading);
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id;
        var strategy = new CombiPricingStrategy();

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = returnMeterReading
        };

        SetupReturnMocks(bookingId, car.Id, categoryId, reservation, car, category, strategy);

        var result = await _reservationService.CreateReturnAsync(returnDto);

        var kmDriven = returnMeterReading - pickupMeterReading; 
        var expectedPrice = (baseDayRental * 1 * 1.3m) + (baseKmPrice * kmDriven); 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldCalculateCorrectPrice_ForTruck_LessThanOneDay_WithKms()
    {
        var baseDayRental = 100m;
        var baseKmPrice = 1m;
        var categoryId = 3;
        var carId = Guid.NewGuid();
        
        var pickupDateTime = DateTime.UtcNow.AddHours(-5);
        var pickupMeterReading = 1000;
        var returnMeterReading = 1500; 
        
        var category = new CarCategory("Truck", CarPricingStrategy.Truck, 1.5m, 1.5m, true, baseKmPrice, baseDayRental);
        var car = new Car("TRUCK1", categoryId, pickupMeterReading, "Scania", "R500", "White", isAvailableToRent: false);
        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        SetReservationCar(reservation, car);
        var bookingId = reservation.Id;
        var strategy = new TruckPricingStrategy();

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = returnMeterReading
        };

        SetupReturnMocks(bookingId, carId, categoryId, reservation, car, category, strategy);

        var result = await _reservationService.CreateReturnAsync(returnDto);

        var kmDriven = returnMeterReading - pickupMeterReading;
        var expectedPrice = (baseDayRental * 1 * 1.5m) + (baseKmPrice * kmDriven * 1.5m); 
        Assert.Equal(expectedPrice, result.RentalAmount);
    }

    [Fact]
    public async Task CreateReturnAsync_ShouldThrow_WhenReservationAlreadyReturned()
    {
        var bookingId = Guid.NewGuid();
        var carId = Guid.NewGuid();
        var pickupDateTime = DateTime.UtcNow.AddHours(-5);
        var pickupMeterReading = 1000;

        var reservation = new Reservation(carId, "1234567890", pickupDateTime, pickupMeterReading);
        reservation.RegisterReturn(pickupDateTime.AddHours(1), pickupMeterReading, 0);

        var returnDto = new ReturnDto
        {
            BookingNumber = bookingId,
            ReturnMeterReading = pickupMeterReading
        };

        _mockReservationRepository
            .Setup(x => x.GetByBookingIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reservationService.CreateReturnAsync(returnDto));
    }

    private void SetupReturnMocks(Guid bookingId,
        Guid carId, 
        int categoryId, 
        Reservation reservation, 
        Car car, CarCategory category, 
        IRentalPricingStrategy rentalPricingStrategy)
    {

        Dictionary<CarPricingStrategy, IRentalPricingStrategy> strategies = new()
        {
            { CarPricingStrategy.SmallCar, new SmallCarPricingStrategy() },
            { CarPricingStrategy.Combi, new CombiPricingStrategy() },
            {CarPricingStrategy.Truck, new TruckPricingStrategy() }
        };

    _mockReservationRepository
            .Setup(x => x.GetByBookingIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        _mockCarRepository
            .Setup(x => x.GetByIdAsync(carId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(car);

        _mockCarCategoryRepository
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _mockReservationRepository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockPricingStrategyFactory.Setup
            (x => x.GetStrategy(It.IsAny<CarPricingStrategy>()))
            .Returns(rentalPricingStrategy);
    }

    private static void SetReservationCar(Reservation reservation, Car car)
    {
        var prop = typeof(Reservation).GetProperty("Car", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        prop?.SetValue(reservation, car);
    }
}
