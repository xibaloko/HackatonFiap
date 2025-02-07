using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;

public class UpdateDoctorRequestHandler : IRequestHandler<UpdateDoctorRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public UpdateDoctorRequestHandler(IUnitOfWork repositories, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = repositories;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("doctor not found."));

        if (identityId != doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        doctor.UpdateBasicInformations(request.Name, request.LastName, request.Email, request.CPF, request.CRM);

        _unitOfWork.DoctorRepository.Update(doctor);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
