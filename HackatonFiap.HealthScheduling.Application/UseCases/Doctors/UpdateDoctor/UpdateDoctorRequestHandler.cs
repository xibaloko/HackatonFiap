using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;

public class UpdateDoctorRequestHandler : IRequestHandler<UpdateDoctorRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDoctorRequestHandler(IUnitOfWork repositories)
    {
        _unitOfWork = repositories;
    }

    public async Task<Result> Handle(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("doctor not found."));

        doctor.UpdateBasicInformations(request.Name, request.LastName, request.Email, request.CPF, request.CRM);

        _unitOfWork.DoctorRepository.Update(doctor);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
