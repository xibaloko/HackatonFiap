using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(entity => entity.Id)
            .HasColumnOrder(1);

        builder.Property(entity => entity.Uuid)
            .ValueGeneratedOnAdd()
            .HasColumnOrder(2);

        builder.HasIndex(entity => entity.Uuid)
            .IsUnique();

        builder.Property(entity => entity.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnOrder(97);

        builder.Property(entity => entity.UpdatedAt)
            .HasColumnOrder(98);

        builder.Property(entity => entity.IsDeleted)
            .HasDefaultValue(false)
            .ValueGeneratedOnAdd()
            .HasColumnOrder(99);
    }
}
