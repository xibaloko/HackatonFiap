using System.Reflection;

namespace HackatonFiap.HealthScheduling.Infrastructure.RabbitMq;

public static class RabbitMqAdapterAssemblyReference
{
    public static readonly Assembly Assembly = typeof(RabbitMqAdapterAssemblyReference).Assembly;
    public static readonly string? Name = Assembly.GetName().Name;
}
