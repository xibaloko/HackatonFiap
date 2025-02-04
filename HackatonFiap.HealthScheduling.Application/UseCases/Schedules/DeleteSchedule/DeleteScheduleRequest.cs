using FluentResults;
using MediatR;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed record DeleteScheduleRequest(Guid Uuid) : IRequest<Result>
{
   
}

