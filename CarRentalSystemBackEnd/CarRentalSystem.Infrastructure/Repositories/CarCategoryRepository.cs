using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories;

public class CarCategoryRepository : ICarCategoryRepository
{
    private readonly CarRentalDbContext _context;

    public CarCategoryRepository(CarRentalDbContext context)
    {
        _context = context;
    }
   
    public  async Task<List<CarCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return  await _context.CarCategories.Where(x => x.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<CarCategory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.CarCategories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// will implement later
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<CarCategory> AddCategoryAsync(CarCategory category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// will implement later
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<CarCategory> UpdateCategoryAsync(CarCategory category, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}

