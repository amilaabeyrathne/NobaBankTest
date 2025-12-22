namespace CarRentalSystem.Domain.PricingStrategies;

public interface IRentalPricingStrategy
{
    decimal CalculatePrice(int days, double km, decimal baseDayRental, decimal baseKmPrice, decimal dayMultiplier, decimal kmMultiplier);
}
