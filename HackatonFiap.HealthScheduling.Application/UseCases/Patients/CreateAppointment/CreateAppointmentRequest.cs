using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;

public sealed record CreateAppointmentRequest(Guid PatientUuid, Guid ScheduleUuid) : IRequest<Result>
{
}
