using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class AppointmentConfiguration : BaseEntityTypeConfiguration<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        base.Configure(builder);
        builder.HasIndex(appointment => appointment.ScheduleId)
            .IsUnique();

        builder.Property(appointment => appointment.PatientId)
            .IsRequired();

        builder.Property(appointment => appointment.ScheduleId)
            .IsRequired();
    }
}