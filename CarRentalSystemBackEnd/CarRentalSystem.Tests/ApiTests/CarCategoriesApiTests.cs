using AutoMapper;
using CarRentalSystem.Api.Controllers;
using CarRentalSystem.Api.Mappings;
using CarRentalSystem.Api.Models.CarCategories;
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRentalSystem.Tests.ApiTests;

public class CarCategoriesApiTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ICarCategoryService> _serviceMock;

    public CarCategoriesApiTests()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiMappingProfile>()).CreateMapper();
        _serviceMock = new Mock<ICarCategoryService>();
    }

    [Fact]
    public async Task Get_Should_Map_CategoryDto_To_Model()
    {
        var dto = new List<CarCategoryDto>
        {
            new() { Id = 1, Name = "Small" },
            new() { Id = 2, Name = "Truck" }
        };
        _serviceMock.Setup(s => s.GetActiveCategoriesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        var controller = new CarCategoriesController(_serviceMock.Object, _mapper);

        var result = await controller.Get(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<List<CarCategoriesModel>>(ok.Value);
        Assert.Collection(body,
            first =>
            {
                Assert.Equal(1, first.Id);
                Assert.Equal("Small", first.Name);
            },
            second =>
            {
                Assert.Equal(2, second.Id);
                Assert.Equal("Truck", second.Name);
            });
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_No_Categories()
    {
        _serviceMock.Setup(s => s.GetActiveCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CarCategoryDto>());

        var controller = new CarCategoriesController(_serviceMock.Object, _mapper);

        var result = await controller.Get(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var body = Assert.IsType<List<CarCategoriesModel>>(ok.Value);
        Assert.Empty(body);
    }
}
