using CarRentalSystem.Domain.PricingStrategies;
using CarRentalSystem.Domain.Primitives;

namespace CarRentalSystem.Application.Factories
{
    public class PricingStrategyFactory
    {
        private readonly Dictionary<CarPricingStrategy, IRentalPricingStrategy> _strategies = new()
        {
            { CarPricingStrategy.SmallCar, new SmallCarPricingStrategy() }, //SMALL
            { CarPricingStrategy.Combi, new CombiPricingStrategy() }, // COMBI
            {CarPricingStrategy.Truck, new TruckPricingStrategy() } //TRUCK
        };

        public IRentalPricingStrategy GetStrategy(CarPricingStrategy pricingStrategy)
        {
            if (_strategies.TryGetValue(pricingStrategy, out var strategy))
                return strategy;

            throw new NotSupportedException($"No pricing strategy defined for category  {pricingStrategy}");
        }
    }
}
