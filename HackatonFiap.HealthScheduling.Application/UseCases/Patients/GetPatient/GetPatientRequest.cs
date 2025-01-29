using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient
{
    public class GetPatientRequest : IRequest<GetPatientResponse>
    {
        public int Id { get; set; }
    }
}
