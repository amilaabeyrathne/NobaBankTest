using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Application.Interfaces.Services;

namespace CarRentalSystem.Application.Services;

public class CarCategoryService : ICarCategoryService
{
    private readonly ICarCategoryRepository _repository;

    public CarCategoryService(ICarCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CarCategoryDto>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetActiveCategoriesAsync(cancellationToken);
        return categories
            .Select(x => new CarCategoryDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();
    }
}

