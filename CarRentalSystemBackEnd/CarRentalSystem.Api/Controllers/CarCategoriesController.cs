using AutoMapper;
using CarRentalSystem.Api.Models.CarCategories;
using CarRentalSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[ApiController]
[Route("categories")]
public class CarCategoriesController : ControllerBase
{
    private readonly ICarCategoryService _service;
    private readonly IMapper _mapper;

    public CarCategoriesController(ICarCategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a list of active car categories.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing this token allows the operation to be canceled.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="List{T}"/> of <see cref="CarCategoriesModel"/>
    /// representing the active car categories, with a status code of 200 (OK) if successful. Returns a status code of
    /// 400 (Bad Request) if the request is invalid.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(CarCategoriesModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var dto = await _service.GetActiveCategoriesAsync(cancellationToken);
        var response = _mapper.Map<List<CarCategoriesModel>>(dto);
        return Ok(response);
    }
}

