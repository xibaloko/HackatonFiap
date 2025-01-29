using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.IoC;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddRabbitAdapter(this IServiceCollection services)
    {
        services.AddScoped<IRabbitRepository, RabbitRepository>();
        return services;
    }
}