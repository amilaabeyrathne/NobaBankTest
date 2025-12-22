using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly CarRentalDbContext _context;

    public ReservationRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation> AddReservationAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
        return reservation;
    }

    public Task<Reservation?> GetByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        return _context.Reservations
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == bookingId, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

