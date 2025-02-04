using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public class ScheduleConfiguration : BaseEntityTypeConfiguration<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);

        builder.Property(schedule => schedule.DateHour)
            .HasColumnOrder(3);

        builder.Property(schedule => schedule.Duration)
            .HasColumnOrder(4);

        builder.Property(schedule => schedule.MedicalAppointmentPrice)
            .HasPrecision(28,2)
            .HasColumnOrder(5);

        builder.Property(schedule => schedule.Avaliable)
            .HasDefaultValue(true)
            .HasColumnOrder(6);

        builder.Property(schedule => schedule.DoctorId)
            .HasColumnOrder(7);       
    }
}
