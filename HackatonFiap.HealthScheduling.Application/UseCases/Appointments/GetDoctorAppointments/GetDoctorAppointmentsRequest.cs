using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetDoctorAppointments;

public sealed record GetDoctorAppointmentsRequest(Guid Uuid) : IRequest<Result<GetDoctorAppointmentsResponse>>
{
}