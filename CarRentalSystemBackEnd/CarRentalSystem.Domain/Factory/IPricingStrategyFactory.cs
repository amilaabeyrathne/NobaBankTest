using CarRentalSystem.Domain.PricingStrategies;
using CarRentalSystem.Domain.Primitives;

namespace CarRentalSystem.Domain.Factory
{
    public interface IPricingStrategyFactory
    {
        IRentalPricingStrategy GetStrategy(CarPricingStrategy pricingStrategy);
    }
}
