using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(AppDbContext db) : base(db)
    {
    }
}
