using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetPatientAppointments;

public sealed record GetPatientAppointmentsRequest(Guid Uuid) : IRequest<Result<GetPatientAppointmentsResponse>>
{
}