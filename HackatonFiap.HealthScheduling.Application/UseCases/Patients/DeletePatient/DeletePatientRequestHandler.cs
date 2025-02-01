using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

public class DeletePatientRequestHandler : IRequestHandler<DeletePatientRequest, Result>
{
    private readonly IRepositories _repositories; 

    public DeletePatientRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(DeletePatientRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        patient.AsSoftDeletable();

        _repositories.PatientRepository.Update(patient);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
