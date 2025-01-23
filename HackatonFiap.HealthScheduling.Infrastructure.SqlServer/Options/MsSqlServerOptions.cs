namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Options;

public sealed class MsSqlServerOptions
{
    public const string SectionName = "MicrosoftSQLServer";
    public required string ConnectionString { get; init; } = "DefaultConnection";
    public required uint CommandTimeoutInSeconds { get; init; } = 60;
    public required bool EnableDetailedErrors { get; init; } = false;
    public required bool EnableSensitiveDataLogging { get; init; } = false;
}
