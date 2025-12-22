namespace CarRentalSystem.Domain.PricingStrategies;

public class SmallCarPricingStrategy : IRentalPricingStrategy
{
    public decimal CalculatePrice(int days, double km, decimal baseDayRental, decimal baseKmPrice, decimal dayMultiplier, decimal kmMultiplier)
    {
       return baseDayRental* days;
    }
}
