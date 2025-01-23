﻿using System.Reflection;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer;

public static class SqlServerAdapterAssemblyReference
{
    public static readonly Assembly Assembly = typeof(SqlServerAdapterAssemblyReference).Assembly;
    public static readonly string? Name = Assembly.GetName().Name;
}
