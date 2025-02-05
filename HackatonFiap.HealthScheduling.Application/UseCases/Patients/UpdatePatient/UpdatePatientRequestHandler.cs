using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatientRequestHandler(IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        patient.UpdateBasicInformations(request.Name, request.LastName, request.Email, request.CPF, request.RG);

        _unitOfWork.PatientRepository.Update(patient);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
