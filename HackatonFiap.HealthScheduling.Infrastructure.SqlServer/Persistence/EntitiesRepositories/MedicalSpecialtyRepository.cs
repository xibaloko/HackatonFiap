using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class MedicalSpecialtyRepository : BaseRepository<MedicalSpecialty>, IMedicalSpecialtyRepository
{
    public MedicalSpecialtyRepository(AppDbContext db) : base(db)
    {
    }
}
