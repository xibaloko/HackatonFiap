using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

public class DeleteDoctorRequestHandler : IRequestHandler<DeleteDoctorRequest, Result>
{
    private readonly IRepositories _repositories; 

    public DeleteDoctorRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));

        doctor.AsSoftDeletable();

        _repositories.DoctorRepository.Update(doctor);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
