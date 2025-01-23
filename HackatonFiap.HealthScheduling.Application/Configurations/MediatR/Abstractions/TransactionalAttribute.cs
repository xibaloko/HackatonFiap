using System.Data;

namespace HackatonFiap.HealthScheduling.Application.Configurations.MediatR.Abstractions;


[AttributeUsage(AttributeTargets.Class)]
public sealed class TransactionalAttribute : Attribute
{
    public IsolationLevel IsolationLevel { get; set; }

    public TransactionalAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        IsolationLevel = isolationLevel;
    }
}
