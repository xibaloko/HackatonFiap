using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed class GetDoctorByUuidRequestHandler : IRequestHandler<GetDoctorByUuidRequest, Result<GetDoctorByUuidResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorByUuidRequestHandler(IUnitOfWork repositories
        , IMapper mapper
        , IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetDoctorByUuidResponse>> Handle(GetDoctorByUuidRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor => doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));

        if (identityId != doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        GetDoctorByUuidResponse response = _mapper.Map<GetDoctorByUuidResponse>(doctor);
        
        return Result.Ok(response);
    }
}
