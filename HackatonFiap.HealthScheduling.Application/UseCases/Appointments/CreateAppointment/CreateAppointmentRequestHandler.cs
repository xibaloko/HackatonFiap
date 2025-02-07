using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.CreateAppointment;

public sealed class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateAppointmentRequestHandler(
        IUnitOfWork repositories,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = repositories;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found!"));

        await _unitOfWork.BeginTransactionAsync(isolationLevel: IsolationLevel.Serializable, cancellationToken);

        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(x => x.Uuid == request.PatientUuid && x.IsDeleted == false, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleNotFound("Patient not found or not avaible!"));

        if (identityId != patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to schedule an appointment."));

        Schedule? schedule = await _unitOfWork.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.ScheduleUuid && x.IsDeleted == false, cancellationToken: cancellationToken);

        if (schedule is null)
            return Result.Fail(ErrorHandler.HandleNotFound("Schedule not found!"));

        if (!schedule.Avaliable)
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not avaliable!"));

        Appointment appointment = new Appointment(patient, schedule);

        await _unitOfWork.AppointmentRepository.AddAsync(appointment, cancellationToken);
        schedule.SetAppointment();
        _unitOfWork.ScheduleRepository.Update(schedule);

        await _unitOfWork.SaveAsync(cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
