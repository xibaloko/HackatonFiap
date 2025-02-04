using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class DoctorConfiguration : BaseEntityTypeConfiguration<Doctor>
{

    public override void Configure(EntityTypeBuilder<Doctor> builder)
    {
        base.Configure(builder);

        builder.Property(doctor => doctor.IdentityId)
            .HasColumnOrder(3);

        builder.Property(doctor => doctor.Name)
            .HasMaxLength(255)
            .HasColumnOrder(4);

        builder.Property(doctor => doctor.LastName)
            .HasMaxLength(255)
            .HasColumnOrder(5);

        builder.Property(doctor => doctor.CPF)
            .HasMaxLength(11)
            .HasColumnOrder(6);

        builder.Property(doctor => doctor.CRM)
            .HasMaxLength(100)
            .HasColumnOrder(7);

        builder.Property(doctor => doctor.Email)
            .HasMaxLength(255)
            .HasColumnOrder(8);

        builder.Property(doctor => doctor.MedicalSpecialtyId)
            .HasColumnOrder(9);
    }
}
