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
            .IsRequired()
            .HasColumnOrder(3);

        builder.HasIndex(schedule => new { schedule.DateHour, schedule.DoctorId })
            .IsUnique();

        builder.Property(schedule => schedule.Duration)
            .IsRequired()
            .HasColumnOrder(4);

        builder.Property(schedule => schedule.Avaliable)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(5);

        builder.Property(schedule => schedule.DoctorId)
            .HasColumnOrder(6);

        builder.Property(schedule => schedule.PatientId)
            .HasColumnOrder(7);
    }
}
