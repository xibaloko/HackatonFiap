using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HackatonFiap.Identity.Application.Configurations.AutoMapper;

public static class AutoMapperConfiguration
{
    public static IServiceCollection AddAutoMapperServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddAutoMapper(assemblies);

        return services;
    }
}
