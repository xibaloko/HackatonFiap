using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseSchedule;

public sealed record RefuseScheduleRequest(
    Guid ScheduleUuid
    ) : IRequest<Result>
{

}
