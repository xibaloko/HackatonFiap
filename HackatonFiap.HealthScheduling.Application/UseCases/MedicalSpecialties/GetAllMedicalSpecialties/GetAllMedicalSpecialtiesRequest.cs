using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public sealed record GetAllMedicalSpecialtiesRequest : IRequest<Result<GetAllMedicalSpecialtiesResponse>>
{
}
