using System.Reflection;

namespace HackatonFiap.Identity.Domain;

public static class DomainAssemblyReference
{
    public static readonly Assembly Assembly = typeof(DomainAssemblyReference).Assembly;
    public static readonly string? Name = Assembly.GetName().Name;
}
