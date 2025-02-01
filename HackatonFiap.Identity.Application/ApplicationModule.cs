using Asp.Versioning.Conventions;
using Asp.Versioning;
using HackatonFiap.Identity.Application.Configurations.AutoMapper;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;
using HackatonFiap.Identity.Application.Configurations.MediatR;
using HackatonFiap.Identity.Application.Services;
using HackatonFiap.Identity.Domain;
using HackatonFiap.Identity.Domain.Services;
using HackatonFiap.Identity.Infrastructure.SqlServer;
using HackatonFiap.Identity.Infrastructure.SqlServer.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.Identity.Application;

public static class ApplicationModule
{
    private const int ApiDefaultMajorVersion = 1;
    private const string ApiVersionHeader = "x-api-version";
    private const string ApiVersionGroupNameFormat = "'v'V";

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
        services.AddInternalDependencies();

        return services;
    }

    private static IServiceCollection AddExternalDependencies(this IServiceCollection services)
    {
        services.AddMediatRServices(SolutionAssemblies)
                .WithValidationPipeline()
                .WithLoggingPipeline();

        services.AddFluentValidationValidators();
        services.AddAutoMapperServices(SolutionAssemblies);

        services.AddApiVersioning(configuration =>
        {
            configuration.DefaultApiVersion = new ApiVersion(ApiDefaultMajorVersion);
            configuration.AssumeDefaultVersionWhenUnspecified = true;
            configuration.ReportApiVersions = true;
            configuration.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader(ApiVersionHeader), new MediaTypeApiVersionReader(ApiVersionHeader));
        }).AddMvc(configuration =>
        {
            configuration.Conventions.Add(new VersionByNamespaceConvention());
        }).AddApiExplorer(configuration =>
        {
            configuration.GroupNameFormat = ApiVersionGroupNameFormat;
            configuration.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection AddInternalDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationTokenService, AuthenticationTokenService>();
        
        return services;
    }
    
    private static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSqlServerAdapter(configuration);
    }
}
