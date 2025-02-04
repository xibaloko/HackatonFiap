using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments.Repositories;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories
{
    public sealed class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext db) : base(db)
        {
        }
    }
}
