using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;

public sealed class GetPatientByUuidRequestHandler : IRequestHandler<GetPatientByUuidRequest, Result<GetPatientByUuidResponse>> 
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientByUuidRequestHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork repositories, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetPatientByUuidResponse>> Handle(GetPatientByUuidRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid && patient.IsDeleted == false, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        if (identityId != patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        GetPatientByUuidResponse response = _mapper.Map<GetPatientByUuidResponse>(patient);

        return Result.Ok(response);
    }

}
