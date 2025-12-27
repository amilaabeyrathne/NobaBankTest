
using CarRentalSystem.Application.Interfaces.Services;
using CarRentalSystem.Application.Services;
using CarRentalSystem.Domain.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPricingStrategyFactory, PricingStrategyFactory>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarCategoryService, CarCategoryService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }
    }
}
