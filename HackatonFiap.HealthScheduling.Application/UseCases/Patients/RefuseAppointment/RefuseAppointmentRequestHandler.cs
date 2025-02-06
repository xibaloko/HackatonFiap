using FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Repositories;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.RefuseAppointment;

public class RefuseAppointmentRequestHandler : IRequestHandler<RefuseAppointmentRequest, Result>
{
    private readonly IRepositories _repositories;

    public RefuseAppointmentRequestHandler(IRepositories repositories)
    {
        _repositories = repositories;
    }

    public async Task<Result> Handle(RefuseAppointmentRequest request, CancellationToken cancellationToken)
    {
        Appointment? appointment = await _repositories.AppointmentRepository.FirstOrDefaultAsync(appointment =>
            appointment.Uuid == request.Uuid &&
            appointment.CancellationReason == request.CancellationReason, cancellationToken: cancellationToken);

        if (appointment is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("appointment not found."));

        appointment.SetCancellation(request.CancellationReason);

        _repositories.DoctorRepository.Update(appointment);
        await _repositories.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
