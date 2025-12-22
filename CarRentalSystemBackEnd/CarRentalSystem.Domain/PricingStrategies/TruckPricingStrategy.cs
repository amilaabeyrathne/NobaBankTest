namespace CarRentalSystem.Domain.PricingStrategies;

public class TruckPricingStrategy : IRentalPricingStrategy
{
    public decimal CalculatePrice(int days, double km, decimal baseDayRental, decimal baseKmPrice, decimal dayMultiplier, decimal kmMultiplier)
    {
        return (baseDayRental * days * dayMultiplier) + (baseKmPrice * (decimal)km * kmMultiplier);
    }
}
