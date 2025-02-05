using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public class GetAllMedicalSpecialtiesRequestHandler : IRequestHandler<GetAllMedicalSpecialtiesRequest, Result<GetAllMedicalSpecialtiesResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMedicalSpecialtiesRequestHandler(IUnitOfWork repositories, IMapper mapper)
    {
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllMedicalSpecialtiesResponse>> Handle(GetAllMedicalSpecialtiesRequest request, CancellationToken cancellationToken)
    {
        var medicalSpecialties = await _unitOfWork.MedicalSpecialtyRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllMedicalSpecialtiesResponse>(medicalSpecialties);

        return Result.Ok(response);
    }
}