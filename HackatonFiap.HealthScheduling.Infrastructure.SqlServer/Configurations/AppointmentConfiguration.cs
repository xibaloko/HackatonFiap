using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public sealed class AppointmentConfiguration : BaseEntityTypeConfiguration<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        base.Configure(builder);

        builder.Property(appointment => appointment.PatientId)
            .HasColumnOrder(3);

        builder.Property(appointment => appointment.ScheduleId)
            .HasColumnOrder(4);

        builder.Property(appointment => appointment.IsCanceledByPatient)
            .HasColumnOrder(5);

        builder.Property(appointment => appointment.CancellationReason)
            .HasMaxLength(255)
            .HasColumnOrder(6);
    }
}