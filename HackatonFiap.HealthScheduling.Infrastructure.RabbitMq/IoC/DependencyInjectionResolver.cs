using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.IoC;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddRabbitAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
        services.AddScoped<IRabbitRepository, RabbitRepository>();
        return services;
    }
}