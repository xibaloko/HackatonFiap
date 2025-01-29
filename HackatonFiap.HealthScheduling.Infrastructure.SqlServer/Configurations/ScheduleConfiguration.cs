using HackatonFiap.HealthScheduling.Domain.Entities.Agendas;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Configurations
{
    public class ScheduleConfiguration : BaseEntityTypeConfiguration<Schedule>
    {
        public override void Configure(EntityTypeBuilder<Schedule> builder)
        {
            base.Configure(builder);

            builder.HasIndex(patient => new { patient.DateHour, patient.DoctorId })
             .IsUnique();

            builder.Property(patient => patient.DateHour)
             .IsRequired();


            builder.Property(patient => patient.Duration)
                .IsRequired();
            
            builder.Property(patient => patient.Avaliable)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
