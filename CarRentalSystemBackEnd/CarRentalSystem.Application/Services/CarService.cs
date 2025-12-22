using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<List<CarResponseDto>> GetCarsByCategoryAsync(int categoryId, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var results = await _carRepository.GetCarAsync(categoryId, page, pageSize, cancellationToken);
            return results.Select(x => new CarResponseDto
            {
                Id = x.Id,
                RegistrationNumber = x.RegistrationNumber,
                CategoryId = x.CategoryId,
                Milage = x.Milage,
                Brand = x.Brand,
                Model = x.Model,
                Colour = x.Colour
            }).ToList();
        }

        public async Task<CarResponseDto?> GetCarByIdAsync(Guid carId, CancellationToken cancellationToken = default)
        {
            var result = await _carRepository.GetByIdAsync(carId, cancellationToken);

            return result == null ? null : new CarResponseDto
            {
                Id = result.Id,
                RegistrationNumber = result.RegistrationNumber,
                CategoryId = result.CategoryId,
                Milage = result.Milage,
                Brand = result.Brand,
                Model = result.Model,
                Colour = result.Colour
            };
        }
    }
}
