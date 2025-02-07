using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAppointmentByUuid;

public sealed record GetAppointmentsByUuidRequest(Guid Uuid) : IRequest<Result<GetAppointmentsByUuidResponse>>
{
}