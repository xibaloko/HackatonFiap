using FluentResults;
using MediatR;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.RefuseAppointment;

public class RefuseAppointmentRequestHandler : IRequestHandler<RefuseAppointmentRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RefuseAppointmentRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RefuseAppointmentRequest request, CancellationToken cancellationToken)
    {
        Appointment? appointment = await _unitOfWork.AppointmentRepository.FirstOrDefaultAsync(appointment =>
            appointment.Uuid == request.AppointmentUuid && appointment.IsDeleted==false, includeProperties: "Schedule", cancellationToken: cancellationToken);

        if (appointment is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("appointment not found."));

        appointment.SetCancellation(request.CancellationReason);

        _unitOfWork.AppointmentRepository.Update(appointment);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
