using CarRentalSystem.Application.Interfaces.Repositories;
using CarRentalSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarCategoryRepository, CarCategoryRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            return services;
        }
    }
}
