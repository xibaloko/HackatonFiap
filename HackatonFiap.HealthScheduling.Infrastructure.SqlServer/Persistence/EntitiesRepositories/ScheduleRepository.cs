using HackatonFiap.HealthScheduling.Domain.Entities.Agendas;
using HackatonFiap.HealthScheduling.Domain.Entities.Agendas.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext db) : base(db)
    {
    }
}
