using HackatonFiap.Identity.Application.Configurations.AutoMapper;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;
using HackatonFiap.Identity.Application.Configurations.MediatR;
using HackatonFiap.Identity.Domain;
using HackatonFiap.Identity.Infrastructure.SqlServer;
using HackatonFiap.Identity.Infrastructure.SqlServer.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.Identity.Application;

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
