using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.RefuseSchedule;

public sealed record RefuseScheduleRequest(
    Guid ScheduleUuid
    ) : IRequest<Result>
{

}
