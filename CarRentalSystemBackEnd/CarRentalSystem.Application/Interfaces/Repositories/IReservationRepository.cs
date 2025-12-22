using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<Reservation> AddReservationAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task<Reservation?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

