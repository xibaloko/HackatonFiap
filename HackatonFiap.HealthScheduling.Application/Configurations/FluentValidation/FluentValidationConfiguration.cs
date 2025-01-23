using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

public static class FluentValidationConfiguration
{
    public static IServiceCollection AddFluentValidationValidators(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }
}