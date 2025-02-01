using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.MediatR;
using HackatonFiap.HealthScheduling.Domain;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.IoC;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HackatonFiap.HealthScheduling.Application;

public static class ApplicationModule
{
    private static readonly Assembly[] SolutionAssemblies =
    [
        ApplicationAssemblyReference.Assembly,
        DomainAssemblyReference.Assembly,
        SqlServerAdapterAssemblyReference.Assembly,
        RabbitMqAdapterAssemblyReference.Assembly
    ];

    public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExternalDependencies();
        services.AddInternalDependencies(configuration);
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

    private static IServiceCollection AddInternalDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:AccessTokenKey"]!))
            };
        });

        return services;
    }

    private static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitAdapter(configuration);
        services.AddSqlServerAdapter(configuration);

        return services;
    }
}
