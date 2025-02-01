using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

public sealed record DeletePatientRequest(Guid Uuid) : IRequest<Result>
{
}
