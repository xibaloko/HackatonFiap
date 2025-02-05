using FluentResults;
using MediatR;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.UpdateSchedule;

public sealed class UpdateScheduleRequest : IRequest<Result>
{
    public Guid Uuid { get; set; }
    public TimeOnly InitialHour { get; init; }
    public TimeOnly FinalHour { get; init; }
    public decimal Price { get; init; }
}

