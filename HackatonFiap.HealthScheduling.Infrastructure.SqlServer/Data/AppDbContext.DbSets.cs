using HackatonFiap.HealthScheduling.Domain.Entities.Agendas;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

public sealed partial class AppDbContext
{
    public DbSet<Doctor> Doctors { get; private set; }
    public DbSet<Patient> Patients { get; private set; }
    public DbSet<Schedule> Schedules { get; private set; }
}
