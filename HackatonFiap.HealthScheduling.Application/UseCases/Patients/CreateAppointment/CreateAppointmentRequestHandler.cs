using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Result>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;
    public readonly IRabbitMqPublisher _raabitRepository;
    public CreateAppointmentRequestHandler(IRepositories repositories
        , IMapper mapper
        , IRabbitMqPublisher rabbitRepository)

    {
        _repositories = repositories;
        _mapper = mapper;
        _raabitRepository = rabbitRepository;
    }

    public async Task<Result> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {

        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(x => x.Uuid == request.PatientUuid
                                                                                      && x.IsDeleted == false
                                                                                      , cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail("Patient not found or not avaible!");


        Schedule? schedule = await _repositories.ScheduleRepository.FirstOrDefaultAsync(x => x.Uuid == request.ScheduleUuid
                                                                                        && x.IsDeleted == false
                                                                                        , cancellationToken: cancellationToken);
        if (schedule is null)
            return Result.Fail("Schedule not found!");

        if (!schedule.Avaliable)
            return Result.Fail("Schedule not avaliable!");


        Appointment appointment = new Appointment(patient, schedule);
        await _repositories.AppointmentRepository.AddAsync(appointment, cancellationToken);
        schedule.SetAppointment();
        _repositories.ScheduleRepository.Update(schedule);
        await _repositories.SaveAsync(cancellationToken);

        var doctor = await _repositories.DoctorRepository.FirstOrDefaultAsync(x => x.Id == schedule.DoctorId, cancellationToken: cancellationToken);
        if (!(doctor is null))
        {
            await _raabitRepository.EnviarMensagem(doctor.Name, doctor.Email, patient.Name, schedule.DateHour.ToString("dd/MM/yyyy"), schedule.DateHour.ToString("HH:mm"));
        }
        return Result.Ok();
    }
}
