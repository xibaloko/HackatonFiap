using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseAppointment;

public class RefuseAppointmentRequestHandler : IRequestHandler<RefuseAppointmentRequest, Result>
{
    private readonly IRepositories _repositories;

    public RefuseAppointmentRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(RefuseAppointmentRequest request, CancellationToken cancellationToken)
    {
        Appointment? appointment = await _repositories.DoctorRepository.FirstOrDefaultAsync(appointmentt =>
            appointment.DoctorUuid == request.DoctorUuid &&
            appointment.ScheduleUuid == request.ScheduleUuid, cancellationToken: cancellationToken);

        if (appointment is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("appointment not found."));

        appointment.UpdateBasicInformations(request.Avaliable);

        _repositories.DoctorRepository.Update(appointment);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
