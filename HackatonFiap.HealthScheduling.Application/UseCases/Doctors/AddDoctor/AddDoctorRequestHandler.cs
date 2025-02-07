using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.IdentityService;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequestHandler : IRequestHandler<AddDoctorRequest, Result<AddDoctorResponse>>
{
    private readonly IApiIdentityService _apiIdentityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddDoctorRequestHandler(IApiIdentityService apiIdentityService, IUnitOfWork repositories, IMapper mapper)
    {
        _apiIdentityService = apiIdentityService;
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<AddDoctorResponse>> Handle(AddDoctorRequest request, CancellationToken cancellationToken)
    {
        Guid identityId = await _apiIdentityService.CreateIdentity(request.CRM, request.Email, request.Password, request.Role);

        if (identityId == Guid.Empty)
            return Result.Fail(ErrorHandler.HandleBadGateway("Unable to create account"));

        try
        {
            var specialty = await _unitOfWork.MedicalSpecialtyRepository.FirstOrDefaultAsync(x => x.Uuid == request.MedicalSpecialtyUuid, cancellationToken: cancellationToken);

            if (specialty is null)
                throw new Exception("Medical Specialty not found!");               

            Doctor doctor = _mapper.Map<Doctor>(request);

            doctor.SetIdentityId(identityId);
            doctor.SetMedicalSpecialty(specialty);

            await _unitOfWork.DoctorRepository.AddAsync(doctor, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            AddDoctorResponse response = _mapper.Map<AddDoctorResponse>(doctor);

            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            await _apiIdentityService.DeleteIdentity(identityId);
            return Result.Fail(ErrorHandler.HandleBadGateway(ex.Message));
        }
    }
}
