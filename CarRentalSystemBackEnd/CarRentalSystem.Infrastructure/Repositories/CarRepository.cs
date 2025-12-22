using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories;

public class CarRepository : ICarRepository
{
    private readonly CarRentalDbContext _context;

    public CarRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car>> GetCarAsync(int categoryId, int page =1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Where(c => c.CategoryId == categoryId && c.IsAvailableToRent == true)
            .OrderBy(c => c.RegistrationNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Car?> GetByIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId, cancellationToken);

    }
}

