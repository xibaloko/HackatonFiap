using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseAppointment;

public sealed record RefuseAppointmentRequest(
    Guid DoctorUuid,
    Guid ScheduleUuid,
    bool Avaliable 
    ) : IRequest<Result>
{
}
