using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

public class DeleteDoctorRequestHandler : IRequestHandler<DeleteDoctorRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork; 

    public DeleteDoctorRequestHandler(IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));

        doctor.AsSoftDeletable();

        _unitOfWork.DoctorRepository.Update(doctor);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
