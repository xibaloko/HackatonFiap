using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid
{
    public sealed record GetPatientByUuidRequest(Guid Uuid) : IRequest<Result<GetPatientByUuidResponse>>
    {
    }
}
