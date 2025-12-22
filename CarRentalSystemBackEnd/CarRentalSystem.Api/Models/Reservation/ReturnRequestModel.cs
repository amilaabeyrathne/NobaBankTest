using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Api.Models.Reservation
{
    public class ReturnRequestModel
    {
        [Required(ErrorMessage = "Booking number is required")]
        public Guid BookingNumber { get; set; }

        [Required(ErrorMessage = "Return meter reading is required")]
        [Range(0, 999999, ErrorMessage = "Meter reading must be between 0 and 999999")]
        public int ReturnMeterReading { get; set; }
        
    }
}
