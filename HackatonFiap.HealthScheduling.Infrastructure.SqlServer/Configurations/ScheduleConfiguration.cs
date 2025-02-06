using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations;

public class ScheduleConfiguration : BaseEntityTypeConfiguration<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);

        builder.Property(schedule => schedule.InitialDateHour)
            .HasColumnOrder(3);

        builder.Property(schedule => schedule.FinalDateHour)
            .HasColumnOrder(4);

        builder.Property(schedule => schedule.MedicalAppointmentPrice)
            .HasPrecision(28, 2)
            .HasColumnOrder(6);

        builder.Property(schedule => schedule.Avaliable)
            .HasDefaultValue(true)
            .HasColumnOrder(7);

        builder.Property(schedule => schedule.DoctorId)
            .HasColumnOrder(8);
    }
}
