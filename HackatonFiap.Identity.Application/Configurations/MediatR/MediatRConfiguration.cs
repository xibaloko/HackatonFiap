using HackatonFiap.Identity.Application.Configurations.MediatR.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.Identity.Application.Configurations.MediatR;

public static class MediatRConfiguration
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies(assemblies);
        });

        return services;
    }

    public static IServiceCollection WithValidationPipeline(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }

    public static IServiceCollection WithLoggingPipeline(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        return services;
    }
}