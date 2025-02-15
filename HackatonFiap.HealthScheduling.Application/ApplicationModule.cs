﻿using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.MediatR;
using HackatonFiap.HealthScheduling.Domain;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HackatonFiap.HealthScheduling.Domain.IdentityService;
using HackatonFiap.HealthScheduling.Application.IdentityService;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using Asp.Versioning.Conventions;
using Asp.Versioning;

namespace HackatonFiap.HealthScheduling.Application;

public static class ApplicationModule
{
    private const int ApiDefaultMajorVersion = 1;
    private const string ApiVersionHeader = "x-api-version";
    private const string ApiVersionGroupNameFormat = "'v'V";

    private static readonly Assembly[] SolutionAssemblies =
    [
        ApplicationAssemblyReference.Assembly,
        DomainAssemblyReference.Assembly,
        SqlServerAdapterAssemblyReference.Assembly
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
        services.AddApiVersioning();
        services.AddSwagger();

        

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

        services.AddScoped<IApiIdentityService, ApiIdentityService>();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddDefaultExceptionHandlers();

        return services;
    }

    private static IServiceCollection AddAdapters(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServerAdapter(configuration);

        return services;
    }

    private static IServiceCollection AddDefaultExceptionHandlers(this IServiceCollection services)
    {
        return services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
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

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Inform the JWT Token"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
