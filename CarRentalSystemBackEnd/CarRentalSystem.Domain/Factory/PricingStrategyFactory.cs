using CarRentalSystem.Domain.PricingStrategies;
using CarRentalSystem.Domain.Primitives;

namespace CarRentalSystem.Domain.Factory
{
    public class PricingStrPricingStrategyFactoryategy : IPricingStrategyFactory
    {
        private readonly Dictionary<CarPricingStrategy, IRentalPricingStrategy> _strategies = new()
        {
            { CarPricingStrategy.SmallCar, new SmallCarPricingStrategy() },
            { CarPricingStrategy.Combi, new CombiPricingStrategy() }, 
            {CarPricingStrategy.Truck, new TruckPricingStrategy() }
        };

        public IRentalPricingStrategy GetStrategy(CarPricingStrategy pricingStrategy)
        {
            if (_strategies.TryGetValue(pricingStrategy, out var strategy))
                return strategy;

            throw new NotSupportedException($"No pricing strategy defined for category  {pricingStrategy}");
        }
    }
}
