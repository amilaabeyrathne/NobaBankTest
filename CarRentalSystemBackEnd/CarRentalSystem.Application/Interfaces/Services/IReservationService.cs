using CarRentalSystem.Application.DTOs;

namespace CarRentalSystem.Application.Interfaces.Services;

public interface IReservationService
{
    Task<ReservationResponseDto> CreateReservationAsync(ReservationDto ReservationDto ,CancellationToken cancellationToken = default);

    Task<ReturnResponseDto> CreateReturnAsync(ReturnDto returnDto, CancellationToken cancellationToken = default);
}
