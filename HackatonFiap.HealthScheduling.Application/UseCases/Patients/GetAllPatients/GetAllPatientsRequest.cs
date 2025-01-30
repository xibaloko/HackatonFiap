using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients
{
    public sealed record GetAllPatientsRequest : IRequest<List<GetAllPatientsResponse>>
    {
    }
}
