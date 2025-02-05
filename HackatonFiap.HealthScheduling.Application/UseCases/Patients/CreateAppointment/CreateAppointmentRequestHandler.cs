using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;

public sealed class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Result>
{
    private readonly IRepositories _repositories;
    private readonly IRabbitMqPublisher _rabbitRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateAppointmentRequestHandler(
        IRepositories repositories,
        IRabbitMqPublisher rabbitRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _repositories = repositories;
        _rabbitRepository = rabbitRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found!"));

        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(x => x.Uuid == request.PatientUuid && x.IsDeleted == false, cancellationToken: cancellationToken);
        
        if (patient is null)
            return Result.Fail(ErrorHandler.HandleNotFound("Patient not found or not avaible!"));

        if (identityId != patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to schedule an appointment."));

        Schedule? schedule = await _repositories.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.ScheduleUuid && x.IsDeleted == false, cancellationToken: cancellationToken);
        
        if (schedule is null)
            return Result.Fail(ErrorHandler.HandleNotFound("Schedule not found!"));

        if (!schedule.Avaliable)
            return Result.Fail(ErrorHandler.HandleBadRequest("Schedule not avaliable!"));

        Appointment appointment = new Appointment(patient, schedule);

        await _repositories.AppointmentRepository.AddAsync(appointment, cancellationToken);
        schedule.SetAppointment();
        _repositories.ScheduleRepository.Update(schedule);
        await _repositories.SaveAsync(cancellationToken);

        var doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(x => x.Id == schedule.DoctorId, cancellationToken: cancellationToken);

        if (doctor is not null)
            await _rabbitRepository.EnviarMensagem(doctor.Name, doctor.Email, patient.Name, schedule.InitialDateHour.ToString("dd/MM/yyyy"), schedule.InitialDateHour.ToString("HH:mm"));

        return Result.Ok();
    }
}
