using CarRentalSystem.Domain.Primitives;
using System;

namespace CarRentalSystem.Domain.Entities;

public class CarCategory
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal DayMultiplier { get; private set; }
    public decimal KmMultiplier { get; private set; }
    public decimal BaseDayRental { get; set; }
    public decimal BaseKmPrice { get; set; }
    public bool IsActive { get; private set; }
    public CarPricingStrategy PricingStrategy { get; private set; }

    public CarCategory(string name, CarPricingStrategy pricingStrategy, decimal dayMultiplier, decimal kmMultiplier,  bool isActive, decimal baseKmPrice , decimal baseDayRental )
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
        if (dayMultiplier <= 0) throw new ArgumentOutOfRangeException(nameof(dayMultiplier));
        if (kmMultiplier < 0) throw new ArgumentOutOfRangeException(nameof(kmMultiplier));

        Name = name.Trim();
        PricingStrategy = pricingStrategy;
        DayMultiplier = dayMultiplier;
        KmMultiplier = kmMultiplier;
        IsActive = isActive;
        BaseKmPrice = baseKmPrice;
        BaseDayRental = baseDayRental;
    }
}



