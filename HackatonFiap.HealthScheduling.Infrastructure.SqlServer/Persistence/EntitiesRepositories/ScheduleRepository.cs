using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext db) : base(db)
    {
    }
}
