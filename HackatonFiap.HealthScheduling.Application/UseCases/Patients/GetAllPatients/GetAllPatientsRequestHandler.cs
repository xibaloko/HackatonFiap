using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

public class GetAllPatientsRequestHandler : IRequestHandler<GetAllPatientsRequest, Result<GetAllPatientsResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetAllPatientsRequestHandler(IHttpContextAccessor httpContextAccessor, IRepositories repositories, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllPatientsResponse>> Handle(GetAllPatientsRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found!"));

        var patients = await _repositories.PatientRepository.GetAllAsync(patient =>
            patient.IsDeleted == false, cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllPatientsResponse>(patients);
        
        return Result.Ok(response);
    }
}
