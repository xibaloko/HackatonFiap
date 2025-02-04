using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequestHandler : IRequestHandler<AddDoctorRequest, Result<AddDoctorResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public AddDoctorRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<AddDoctorResponse>> Handle(AddDoctorRequest request, CancellationToken cancellationToken)
    {
        var specialty = await _repositories.MedicalSpecialtyRepository.FirstOrDefaultAsync(x => x.Uuid == request.MedicalSpecialtyUuid, cancellationToken: cancellationToken);

        if (specialty is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Medical Specialty not found!"));

        Doctor doctor = _mapper.Map<Doctor>(request);

        doctor.SetMedicalSpecialty(specialty);
        await _repositories.DoctorRepository.AddAsync(doctor, cancellationToken);
        await _repositories.SaveAsync(cancellationToken);

        AddDoctorResponse response = _mapper.Map<AddDoctorResponse>(doctor);

        return Result.Ok(response);
    }
}
