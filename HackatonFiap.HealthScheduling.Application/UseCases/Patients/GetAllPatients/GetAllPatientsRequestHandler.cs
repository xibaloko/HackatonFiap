using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, Result<UpdatePatientResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public UpdatePatientRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<UpdatePatientResponse>> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        var patients = await _repositories.PatientRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = _mapper.Map<UpdatePatientResponse>(patients);
        
        return Result.Ok(response);
    }
}
