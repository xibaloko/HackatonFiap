using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.RefuseAppointment;

public sealed record RefuseAppointmentRequest(
    Guid Uuid,
    string CancellationReason
    ) : IRequest<Result>
{
}
