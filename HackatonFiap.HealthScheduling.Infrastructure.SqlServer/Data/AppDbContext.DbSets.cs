using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

public sealed partial class AppDbContext
{
    public DbSet<Doctor> Doctors { get; private set; }
}
