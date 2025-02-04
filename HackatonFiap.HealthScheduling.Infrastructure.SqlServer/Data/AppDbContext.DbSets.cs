using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

public sealed partial class AppDbContext
{
    public DbSet<Doctor> Doctors { get; private set; }
    public DbSet<Patient> Patients { get; private set; }
    public DbSet<Schedule> Schedules { get; private set; }
    public DbSet<Appointment> Appointments { get; private set; }
    public DbSet<MedicalSpecialty> MedicalSpecialties { get; private set; }
}
