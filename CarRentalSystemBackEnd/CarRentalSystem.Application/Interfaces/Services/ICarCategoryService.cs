using CarRentalSystem.Application.DTOs;

namespace CarRentalSystem.Application.Interfaces.Services;

public interface ICarCategoryService
{
    Task<List<CarCategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}
