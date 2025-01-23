using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(entity => entity.Uuid)
            .ValueGeneratedOnAdd();

        builder.HasIndex(entity => entity.Uuid)
            .IsUnique();

        builder.Property(entity => entity.CreatedAt)
            .ValueGeneratedOnAdd();

        builder.Property(entity => entity.IsDeleted)
            .HasDefaultValue(false)
            .ValueGeneratedOnAdd();
    }
}
