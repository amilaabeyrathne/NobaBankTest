using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces.Repositories;

public interface ICarCategoryRepository
{
   Task<List<CarCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
   Task<CarCategory> AddCategoryAsync(CarCategory category, CancellationToken cancellationToken = default);
   Task<CarCategory> UpdateCategoryAsync(CarCategory category, CancellationToken cancellationToken = default);
   Task<CarCategory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
