using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientById
{
    public class GetPatientByIdRequest : IRequest<GetPatientByIdResponse>
    {
        public int Id { get; set; }
    }
}
