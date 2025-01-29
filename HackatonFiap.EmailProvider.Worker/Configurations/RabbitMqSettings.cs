namespace HackatonFiap.EmailProvider.Worker.Configurations;

public class RabbitMqSettings
{
    public string QueueName { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
}