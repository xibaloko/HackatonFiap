using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class PatientConfiguration : BaseEntityTypeConfiguration<Patient>
{

    public override void Configure(EntityTypeBuilder<Patient> builder)
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
