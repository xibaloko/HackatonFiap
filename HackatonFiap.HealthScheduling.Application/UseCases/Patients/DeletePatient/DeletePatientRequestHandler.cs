using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

public class DeletePatientRequestHandler : IRequestHandler<DeletePatientRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork; 

    public DeletePatientRequestHandler(IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(DeletePatientRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        patient.AsSoftDeletable();

        _unitOfWork.PatientRepository.Update(patient);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
