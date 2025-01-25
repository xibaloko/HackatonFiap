using HackatonFiap.HealthScheduling.Domain.Entities.Bases.Repositories;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces
{
    public interface IPatientRepository : IBaseRepository<Patient>
    {
    }
}
