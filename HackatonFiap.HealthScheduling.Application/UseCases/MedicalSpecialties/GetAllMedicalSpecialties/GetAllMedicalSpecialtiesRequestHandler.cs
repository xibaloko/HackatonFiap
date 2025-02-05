using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public class GetAllMedicalSpecialtiesRequestHandler : IRequestHandler<GetAllMedicalSpecialtiesRequest, Result<GetAllMedicalSpecialtiesResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetAllMedicalSpecialtiesRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllMedicalSpecialtiesResponse>> Handle(GetAllMedicalSpecialtiesRequest request, CancellationToken cancellationToken)
    {
        var medicalSpecialties = await _repositories.MedicalSpecialtyRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllMedicalSpecialtiesResponse>(medicalSpecialties);

        return Result.Ok(response);
    }
}