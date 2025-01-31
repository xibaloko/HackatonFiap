using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

public sealed record GetAllPatientsRequest : IRequest<Result<GetAllPatientsResponse>>
{
}
