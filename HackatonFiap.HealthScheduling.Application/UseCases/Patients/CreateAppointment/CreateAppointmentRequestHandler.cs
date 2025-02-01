using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Result>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public CreateAppointmentRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
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
        await _repositories.AppointmentRepository.AddAsync(appointment,cancellationToken);
        schedule.SetAppointment();
        _repositories.ScheduleRepository.Update(schedule);
        await _repositories.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
