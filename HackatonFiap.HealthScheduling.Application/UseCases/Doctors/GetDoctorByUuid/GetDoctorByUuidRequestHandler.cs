using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed class GetDoctorByUuidRequestHandler : IRequestHandler<GetDoctorByUuidRequest, Result<GetDoctorByUuidResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetDoctorByUuidRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetDoctorByUuidResponse>> Handle(GetDoctorByUuidRequest request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(doctor => doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));

        GetDoctorByUuidResponse response = _mapper.Map<GetDoctorByUuidResponse>(doctor);
        
        return Result.Ok(response);
    }
}
