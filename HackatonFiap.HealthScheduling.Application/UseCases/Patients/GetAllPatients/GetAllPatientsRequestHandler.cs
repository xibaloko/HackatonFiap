using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

public class GetAllPatientsRequestHandler : IRequestHandler<GetAllPatientsRequest, Result<GetAllPatientsResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetAllPatientsRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllPatientsResponse>> Handle(GetAllPatientsRequest request, CancellationToken cancellationToken)
    {
        var patients = await _repositories.PatientRepository.GetAllAsync(patient =>
            patient.IsDeleted == false, cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllPatientsResponse>(patients);
        
        return Result.Ok(response);
    }
}
