namespace CarRentalSystem.Application.DTOs;

public class ReturnDto
{
    public Guid BookingNumber { get; set; }
    public int ReturnMeterReading { get; set; }
    public decimal CalculatedPrice { get; set; }
}
