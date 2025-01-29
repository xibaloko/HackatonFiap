namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;

internal sealed class RabbitMqSettings
{
    public const string Section = "RabbitMQ";
    public string QueueName { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
}