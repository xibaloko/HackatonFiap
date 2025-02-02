using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.RabbitMqPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.IoC;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddRabbitAdapter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRabbitMqOptions(configuration);

        services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
        return services;
    }

    private static IServiceCollection AddRabbitMqOptions(
        this IServiceCollection services,
        IConfiguration configuration) 
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.Section));
        return services;
    }
}