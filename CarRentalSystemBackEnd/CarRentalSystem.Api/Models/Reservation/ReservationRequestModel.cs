using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Api.Models.Reservation
{
    public class ReservationRequestModel
    {
        [Required(ErrorMessage = "Car ID is required ")]
        public Guid CarId { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^(\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])|\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01]))[-]?\d{4}$",
         ErrorMessage = "Invalid SSN format. Use YYYYMMDD-XXXX or YYMMDD-XXXX")] // Swedish SSN format
        public string CustomerSocialSecurityNumber { get; set; }

        [Required(ErrorMessage = "Meter Reading is Required")]
        [Range(0, 999999, ErrorMessage = "Meter reading must be between 0 and 999999")]
        public int PickupMeterReading { get; set; }
    }
}
