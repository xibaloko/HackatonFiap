using FluentResults;
using MediatR;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed class AddScheduleRequest : IRequest<Result>
{
    public Guid DoctorUuid { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly InitialHour { get; init; }
    public TimeOnly FinalHour { get; init; }
    public int Duration { get; init; }
    public bool Avaliable { get; init; }
}

