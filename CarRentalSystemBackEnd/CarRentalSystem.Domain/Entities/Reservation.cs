using System;

namespace CarRentalSystem.Domain.Entities;

public class Reservation
{
    public Guid Id { get; private set; } // booking number 
    public Guid CarId { get; private set; } // Registration number?
    public string CustomerSocialSecurityNumber { get; private set; }
    public DateTime PickupDateTime { get; private set; }
    public int PickupMeterReading { get; private set; }
    public DateTime? ReturnDateTime { get; private set; }
    public int? ReturnMeterReading { get; private set; }
    public decimal? CalculatedPrice { get; private set; }
    public bool IsReturned { get; private set; }
    public Car? Car { get; private set; }

    public Reservation( Guid carId, string customerSocialSecurityNumber, DateTime pickupDateTime, int pickupMeterReading)
    {
        if (carId == Guid.Empty) throw new ArgumentException("car Id should not be empty");
        if (pickupMeterReading < 0) throw new ArgumentOutOfRangeException("Pick up meter reading should not be less than Zero");

        Id = Guid.NewGuid();
        CarId = carId;
        CustomerSocialSecurityNumber = customerSocialSecurityNumber.Trim();
        PickupDateTime = pickupDateTime;
        PickupMeterReading = pickupMeterReading;
        IsReturned = false;

    }

    public void RegisterReturn(DateTime returnDateTime, int returnMeterReading, decimal calculatedPrice)
    {
        if (IsReturned) throw new InvalidOperationException("Reservation already returned");
        if (returnDateTime < PickupDateTime) throw new ArgumentOutOfRangeException("Return Date must be greater than pick up date");
        if (returnMeterReading < PickupMeterReading) throw new ArgumentOutOfRangeException("Return meter reading must be greater than or equal to prick up meter reading");
        if (calculatedPrice < 0) throw new ArgumentOutOfRangeException("Final amount must be none negative");

        ReturnDateTime = returnDateTime;
        ReturnMeterReading = returnMeterReading;
        CalculatedPrice = calculatedPrice;
        IsReturned = true;
    }
}

