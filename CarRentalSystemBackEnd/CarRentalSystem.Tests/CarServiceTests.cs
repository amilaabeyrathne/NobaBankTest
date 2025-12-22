using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Domain.Entities;
using Moq;
using Xunit;

namespace CarRentalSystem.Tests;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _mockCarRepository;
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _mockCarRepository = new Mock<ICarRepository>();
        _carService = new CarService(_mockCarRepository.Object);
    }

    [Fact]
    public async Task GetCarsByCategoryAsync_ShouldCallRepositoryWithCorrectParameters()
    {
        var categoryId = 1;
        var page = 1;
        var pageSize = 10;
        var expectedCars = new List<Car>
        {
            new Car("ABC123", categoryId, 1000, "Toyota", "Camry", "Blue"),
            new Car("XYZ789", categoryId, 2000, "Honda", "Civic", "Red")
        };

        _mockCarRepository
            .Setup(x => x.GetCarAsync(categoryId, page, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCars);
        var result = await _carService.GetCarsByCategoryAsync(categoryId, page, pageSize);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockCarRepository.Verify(x => x.GetCarAsync(categoryId, page, pageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCarsByCategoryAsync_ShouldUseDefault_WhenNotProvided()
    {
        var categoryId = 1;
        var expectedCars = new List<Car>();

        _mockCarRepository
            .Setup(x => x.GetCarAsync(categoryId, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCars);

        await _carService.GetCarsByCategoryAsync(categoryId);

        _mockCarRepository.Verify(x => x.GetCarAsync(categoryId, 1, 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCarsByCategoryAsync_ShouldPassCancellationTokenToRepository()
    {
        var categoryId = 1;
        var cancellationToken = new CancellationToken(true);
        var expectedCars = new List<Car>();

        _mockCarRepository
            .Setup(x => x.GetCarAsync(categoryId, 1, 10, cancellationToken))
            .ReturnsAsync(expectedCars);

        await _carService.GetCarsByCategoryAsync(categoryId, cancellationToken: cancellationToken);
        _mockCarRepository.Verify(x => x.GetCarAsync(categoryId, 1, 10, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetCarsByCategoryAsync_ShouldReturnEmptyList_WhenNoCarsFound()
    {
        var categoryId = 1;
        var emptyList = new List<Car>();

        _mockCarRepository
            .Setup(x => x.GetCarAsync(categoryId, 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        var result = await _carService.GetCarsByCategoryAsync(categoryId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

