using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class DoctorConfiguration : BaseEntityTypeConfiguration<Doctor>
{

    public override void Configure(EntityTypeBuilder<Doctor> builder)
    {
        base.Configure(builder);

        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(customer => customer.LastName)
            .IsRequired()
            .HasMaxLength(200);
    }
}
