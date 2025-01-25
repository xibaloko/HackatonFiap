using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class AddPatientRequestHandler : IRequestHandler<AddPatientRequest, Result<AddPatientResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public AddPatientRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<AddPatientResponse>> Handle(AddPatientRequest request, CancellationToken cancellationToken)
    {
        Patient patient = _mapper.Map<Patient>(request);

        await _repositories.PatientRepository.AddAsync(patient, cancellationToken);
        await _repositories.SaveAsync(cancellationToken);

        AddPatientResponse response = _mapper.Map<AddPatientResponse>(patient);

        return Result.Ok(response);
    }
}
