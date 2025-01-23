using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.MediatR;
using HackatonFiap.HealthScheduling.Domain;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.HealthScheduling.Application;

public static class ApplicationModule
{
    private static readonly Assembly[] SolutionAssemblies =
    [
        ApplicationAssemblyReference.Assembly,
        DomainAssemblyReference.Assembly,
        SqlServerAdapterAssemblyReference.Assembly,
    ];

    public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExternalDependencies();
        services.AddAdapters(configuration);

        return services;
    }

    private static IServiceCollection AddExternalDependencies(this IServiceCollection services)
    {
        services.AddMediatRServices(SolutionAssemblies)
                .WithValidationPipeline()
                .WithLoggingPipeline();

        services.AddFluentValidationValidators();
        services.AddAutoMapperServices(SolutionAssemblies);

        return services;
    }

    private static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSqlServerAdapter(configuration);
    }
}
