using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, Result>
{
    private readonly IRepositories _repositories;

    public UpdatePatientRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        patient.UpdateBasicInformations(request.Name, request.LastName, request.Email, request.CPF, request.RG);

        _repositories.PatientRepository.Update(patient);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
