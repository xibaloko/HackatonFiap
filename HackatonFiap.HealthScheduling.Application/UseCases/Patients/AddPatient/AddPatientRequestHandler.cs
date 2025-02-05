using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.IdentityService;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class AddPatientRequestHandler : IRequestHandler<AddPatientRequest, Result<AddPatientResponse>>
{
    
    private readonly IApiIdentityService _apiIdentityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddPatientRequestHandler(IApiIdentityService apiIdentityService, IUnitOfWork repositories, IMapper mapper)
    {
        _apiIdentityService = apiIdentityService;
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<AddPatientResponse>> Handle(AddPatientRequest request, CancellationToken cancellationToken)
    {
        Guid identityId = await _apiIdentityService.CreateIdentity(request.Username, request.Email, request.Password, request.Role);

        if (identityId == Guid.Empty)
            return Result.Fail(ErrorHandler.HandleBadGateway("Unable to create account"));

        Patient patient = _mapper.Map<Patient>(request);

        patient.SetIdentityId(identityId);

        await _unitOfWork.PatientRepository.AddAsync(patient, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        AddPatientResponse response = _mapper.Map<AddPatientResponse>(patient);

        return Result.Ok(response);
    }


}
