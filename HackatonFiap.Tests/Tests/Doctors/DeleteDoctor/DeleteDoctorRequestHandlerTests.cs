using System.Security.Claims;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HackatonFiap.Tests.Tests.Doctors.DeleteDoctor;

public class DeleteDoctorRequestHandler : IRequestHandler<DeleteDoctorRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public DeleteDoctorRequestHandler(
        IUnitOfWork repositories
        , IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));
        
        if (identityId != doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        doctor.AsSoftDeletable();

        _unitOfWork.DoctorRepository.Update(doctor);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}