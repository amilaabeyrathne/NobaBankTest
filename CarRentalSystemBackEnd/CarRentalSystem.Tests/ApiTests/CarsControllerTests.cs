using AutoMapper;
using CarRentalSystem.Api.Mappings;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.DTOs;
using Moq;
using Xunit;
using CarRentalSystem.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Api.Models.Cars;


namespace CarRentalSystem.Tests.ApiTests
{
    
    public class CarsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICarService> _serviceMock;

        public CarsControllerTests( )
        {
            _mapper =new MapperConfiguration(cfg => cfg.AddProfile<ApiMappingProfile>()).CreateMapper();
            _serviceMock = new Mock<ICarService>();
        }

        [Fact]
        public async Task GetByCategory_Should_Call_Service_And_Map_Result()
        {
            var categoryId = 1;
            var page = 1;
            var pageSize = 10;
            var expectedCars = new List<CarResponseDto>
            {
                new CarResponseDto { Id = Guid.NewGuid(), RegistrationNumber = "ABC123", Brand = "Toyota", Model = "Camry", Colour = "Blue" },
                new CarResponseDto { Id = Guid.NewGuid(), RegistrationNumber = "XYZ789", Brand = "Honda", Model = "Civic", Colour = "Red" }
            };

            _serviceMock
                .Setup(s => s.GetCarsByCategoryAsync(categoryId, page, pageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCars);

            var controller = new CarsController(_serviceMock.Object, _mapper);
            var result = await controller.GetByCategory(categoryId, page, pageSize, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsType<List<CarModel>>(okResult.Value);

            Assert.Equal(2, body.Count);

            _serviceMock.Verify(s => s.GetCarsByCategoryAsync(categoryId, page, pageSize, It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
