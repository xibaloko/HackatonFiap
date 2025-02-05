using FluentResults;
using MediatR;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddAppointmentSlot;

public sealed class AddAppointmentSlotRequest : IRequest<Result>
{
    public Guid DoctorUuid { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly InitialHour { get; init; }    
    public int Duration { get; init; }    
    public decimal Price { get; init; }
}

