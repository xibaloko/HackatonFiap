using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;

public sealed record GetPatientByUuidRequest(Guid Uuid) : IRequest<Result<GetPatientByUuidResponse>>
{
}
