using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

public sealed record GetScheduleFromDoctorRequest(Guid DoctorUuId) : IRequest<Result<GetScheduleFromDoctorResponse>>
{
}
