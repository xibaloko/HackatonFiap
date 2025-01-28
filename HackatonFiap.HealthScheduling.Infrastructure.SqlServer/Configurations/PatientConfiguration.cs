using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class PatientConfiguration : BaseEntityTypeConfiguration<Patient>
{

    public override void Configure(EntityTypeBuilder<Patient> builder)
    {
        base.Configure(builder);

        builder.Property(patient => patient.Name)
           .IsRequired()
           .HasMaxLength(255);

        builder.Property(patient => patient.LastName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(patient => patient.CPF)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(patient => patient.RG)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(patient => patient.Email)
            .IsRequired()
            .HasMaxLength(255);
    }
}
