using AutoMapper;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Api.Controllers
{
    
    [ApiController]
    [Route("cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarsController(ICarService carService ,IMapper mapper )
        {
            _carService = carService;
            _mapper = mapper;
        }

        /// <summary>
        ///  method to get cars by category with pagination.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(CarModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCategory([FromQuery,Required] int categoryId, [FromQuery,Range(1,10)] int page = 1,
            [FromQuery, Range(1,10)] int pageSize = 1 , CancellationToken cancellationToken = default)
        {
            var cars = await _carService.GetCarsByCategoryAsync(categoryId, page, pageSize, cancellationToken);
            var response = _mapper.Map<List<CarModel>>(cars);
            return Ok(response);
        }

        /// <summary>
        /// method to get car by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet ("{id}")]
        [ProducesResponseType(typeof(CarModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCarById(Guid id, CancellationToken cancellationToken)
        {
            var car = await _carService.GetCarByIdAsync(id, cancellationToken);
            var response = _mapper.Map<CarModel>(car);
            return Ok(response);
        }
    }
}
