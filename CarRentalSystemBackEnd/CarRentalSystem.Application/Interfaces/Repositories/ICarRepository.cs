using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces.Repositories;

public interface ICarRepository
{
    Task<List<Car>> GetCarAsync(int categoryId, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Car?> GetByIdAsync(Guid carId, CancellationToken cancellationToken = default);
}
