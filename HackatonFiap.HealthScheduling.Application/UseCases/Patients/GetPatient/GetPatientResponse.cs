using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient
{
    public class GetPatientResponse
    {
        public int Id { get; init; }
        public Guid Uuid { get; init; }
        public string Name { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string CPF { get; init; }
        public string RG { get; init; }
    }
}
