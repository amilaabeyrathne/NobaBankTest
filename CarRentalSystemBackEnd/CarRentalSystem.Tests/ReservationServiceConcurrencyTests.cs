using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Factory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CarRentalSystem.Tests;

public class ReservationServiceConcurrencyTests
{
    private readonly Mock<ICarRepository> _mockCarRepository;
    private readonly Mock<IReservationRepository> _mockReservationRepository;
    private readonly Mock<ICarCategoryRepository> _mockCarCategoryRepository;
    private readonly Mock<IPricingStrategyFactory>   _pricingStrategyFactory;
    private readonly ReservationService _reservationService;
    private readonly Mock<ILogger<ReservationService>> _mockLogger;

    public ReservationServiceConcurrencyTests()
    {
        _mockCarRepository = new Mock<ICarRepository>();
        _mockReservationRepository = new Mock<IReservationRepository>();
        _mockCarCategoryRepository = new Mock<ICarCategoryRepository>();
        _mockLogger = new Mock<ILogger<ReservationService>>();
        _pricingStrategyFactory = new Mock<IPricingStrategyFactory>();
        _reservationService = new ReservationService(
            _mockCarRepository.Object,
            _mockReservationRepository.Object,
            _pricingStrategyFactory.Object,
            _mockCarCategoryRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenDatabaseUniqueConstraintViolationOccurs()
    {
        var carId = Guid.NewGuid();
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

        var constraintException = new Exception("UNIQUE constraint failed");
        _mockReservationRepository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(constraintException);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _reservationService.CreateReservationAsync(reservationDto));

        Assert.Contains("Car may not available to rent. Please try again later.", ex.Message);
        Assert.NotNull(ex.InnerException);
        Assert.Contains("UNIQUE constraint failed", ex.InnerException.Message);
    }
    
}

