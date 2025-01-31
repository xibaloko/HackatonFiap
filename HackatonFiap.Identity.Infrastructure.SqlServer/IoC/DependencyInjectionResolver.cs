using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Infrastructure.SqlServer.Data;
using HackatonFiap.Identity.Infrastructure.SqlServer.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackatonFiap.Identity.Infrastructure.SqlServer.IoC;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddSqlServerAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkCore(configuration);
        services.AddIdentity();

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

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>()
          .AddDefaultTokenProviders();

        return services;
    }
}
