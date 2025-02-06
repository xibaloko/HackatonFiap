using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Initializer;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Options;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.IoC;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddSqlServerAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkCore(configuration);
        services.AddRepositories();
        services.AddDbInitializer();

        return services;
    }

    private static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((provider, builder) =>
        {
            MsSqlServerOptions sqlServerOptions = provider.GetRequiredService<IOptions<MsSqlServerOptions>>().Value;

            builder.UseSqlServer(configuration.GetConnectionString(sqlServerOptions.ConnectionString), optionsBuilder =>
            {
                optionsBuilder.CommandTimeout((int)sqlServerOptions.CommandTimeoutInSeconds);
            });

            builder.EnableDetailedErrors(sqlServerOptions.EnableDetailedErrors);
        }, contextLifetime: ServiceLifetime.Scoped);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDbInitializer(this IServiceCollection services)
    {
        services.AddTransient<IDbInitializer, DbInitializer>();

        return services;
    }
}
