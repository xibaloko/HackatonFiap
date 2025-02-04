using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public class MedicalSpecialtyConfiguration : BaseEntityTypeConfiguration<MedicalSpecialty>
{
    public override void Configure(EntityTypeBuilder<MedicalSpecialty> builder)
    {
        base.Configure(builder);

        builder.Property(specialty => specialty.Description)
            .HasMaxLength(255)
            .HasColumnOrder(3);
    }
}
