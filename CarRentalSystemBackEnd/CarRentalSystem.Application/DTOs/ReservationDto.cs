namespace CarRentalSystem.Application.DTOs;

public class ReservationDto
{
    public Guid CarId { get; set; }
    public string CustomerSocialSecurityNumber { get; set; }
    public int PickupMeterReading { get; set; }
}
