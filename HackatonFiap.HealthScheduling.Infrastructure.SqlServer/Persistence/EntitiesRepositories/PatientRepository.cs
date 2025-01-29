using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class PatientRepository : BaseRepository<Patient>, IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext db) : base(db)
    {
        _context = db;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }
}
