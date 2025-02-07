using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdatePatientRequestHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork repositories, IMapper mapper)
    {
        _unitOfWork = repositories;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));
        
        if (identityId != patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        patient.UpdateBasicInformations(request.Name, request.LastName, request.Email, request.CPF, request.RG);

        _unitOfWork.PatientRepository.Update(patient);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
