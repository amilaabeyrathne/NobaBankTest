using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces.Services
{
    public interface ICarService
    {
        Task<List<CarResponseDto>> GetCarsByCategoryAsync(int categoryId, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<CarResponseDto?> GetCarByIdAsync(Guid carId, CancellationToken cancellationToken = default);
    }
}
