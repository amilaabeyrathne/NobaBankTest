using AutoMapper;
using CarRentalSystem.Api.Models.Reservation;
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[ApiController]
[Route("reservation")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationsController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new reservation based on the provided request data.
    /// </summary>
    /// <remarks>This method maps the incoming reservation request to a data transfer object, processes the
    /// reservation creation through the service layer, and returns the result as a response model.</remarks>
    /// <param name="reservationRequest">The reservation details provided by the client.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IActionResult"/> containing the created reservation's unique identifier if the operation is
    /// successful, or a bad request response if the input is invalid.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ReservationRequestModel reservationRequest, CancellationToken cancellationToken)
    {
        var reservationDto = _mapper.Map<ReservationDto>(reservationRequest);
        var result = await _reservationService.CreateReservationAsync(reservationDto, cancellationToken);
        var response = _mapper.Map<ReservationResponseModel>(result);
        return Ok(response);
    }

    /// <summary>
    /// Registers a return for a reservation and processes the associated return details.
    /// </summary>
    /// <remarks>This method maps the provided return request to a data transfer object, processes the return
    /// using the reservation service, and returns the result as a response model. Ensure that the <paramref
    /// name="returnRequest"/> contains valid data to avoid a 400 Bad Request response.</remarks>
    /// <param name="returnRequest">The details of the return, including reservation and item information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="ReturnResponseModel"/> with the processed return details
    /// if the operation is successful, or a 400 Bad Request response if the input is invalid.</returns>
    [HttpPost("return")]
    [ProducesResponseType(typeof(ReturnResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterReturn([FromBody] ReturnRequestModel returnRequest, CancellationToken cancellationToken)
    {
        var returnDto = _mapper.Map<ReturnDto>(returnRequest);
        var result = await _reservationService.CreateReturnAsync(returnDto, cancellationToken);
        var response = _mapper.Map<ReturnResponseModel>(result);
        return Ok(response);
    }
}

