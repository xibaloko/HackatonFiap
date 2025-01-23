using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

public sealed partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleModifiedEntries();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleModifiedEntries()
    {
        IEnumerable<EntityEntry<EntityBase>> entries = ChangeTracker.Entries<EntityBase>().Where(entry => entry.State is EntityState.Modified);

        foreach (EntityEntry<EntityBase> entry in entries)
            entry.Entity.UpdatedNow();
    }
}
