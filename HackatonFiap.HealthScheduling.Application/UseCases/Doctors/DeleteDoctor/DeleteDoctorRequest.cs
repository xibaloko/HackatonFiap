using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

public sealed record DeleteDoctorRequest(Guid Uuid) : IRequest<Result>
{
}
