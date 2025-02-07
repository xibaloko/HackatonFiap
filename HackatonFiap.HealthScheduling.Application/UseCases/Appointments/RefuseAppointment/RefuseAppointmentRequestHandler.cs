using FluentResults;
using MediatR;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.RefuseAppointment;

public class RefuseAppointmentRequestHandler : IRequestHandler<RefuseAppointmentRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefuseAppointmentRequestHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(RefuseAppointmentRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found!"));

        Appointment? appointment = await _unitOfWork.AppointmentRepository.FirstOrDefaultAsync(appointment =>
            appointment.Uuid == request.AppointmentUuid && appointment.IsDeleted == false, includeProperties: "Schedule,Patient", cancellationToken: cancellationToken);

        if (appointment is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("appointment not found."));
       
        if (identityId != appointment.Patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to schedule an appointment."));

        appointment.SetCancellation(request.CancellationReason);

        _unitOfWork.AppointmentRepository.Update(appointment);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
