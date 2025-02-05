using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

public class GetScheduleFromDoctorHandler : IRequestHandler<GetScheduleFromDoctorRequest, Result<GetScheduleFromDoctorResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetScheduleFromDoctorHandler(
        IRepositories repositories
        , IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetScheduleFromDoctorResponse>> Handle(GetScheduleFromDoctorRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Schedule> schedules = await _repositories.ScheduleRepository.GetAllAsync(x =>
                                                            x.Avaliable == true
                                                            && x.IsDeleted == false
                                                            && x.Doctor.Uuid == request.DoctorUuId
                                                            , cancellationToken: cancellationToken);

        var response = CreateResponse(request, schedules);

        return Result.Ok(response);
    }

    private static GetScheduleFromDoctorResponse CreateResponse(GetScheduleFromDoctorRequest request, IEnumerable<Schedule> schedules)
    {
        var schedulesGrouped = schedules.GroupBy(x => x.InitialDateHour.Date).ToList();

        GetScheduleFromDoctorResponse getScheduleFromDoctorResponse = new GetScheduleFromDoctorResponse
        {
            DoctorUuid = request.DoctorUuId
        };
        List<DoctorAvailableSchedule> doctorAvailableSchedules = new List<DoctorAvailableSchedule>();

        foreach (var schedule in schedulesGrouped)
        {
            var doctorAvailableSchedule = new DoctorAvailableSchedule
            {
                DateSchedule = DateOnly.Parse(schedule.Key.Date.ToShortDateString())
            };
            doctorAvailableSchedule.Appointments = new List<Appointment>();

            foreach (var hour in schedule)
            {
                var appointment = new Appointment
                {
                    Hour = TimeOnly.Parse(hour.InitialDateHour.TimeOfDay.ToString()),
                    ScheduleUuid = hour.Uuid
                };
                doctorAvailableSchedule.Appointments.Add(appointment);
            }
            doctorAvailableSchedules.Add(doctorAvailableSchedule);
        }
        getScheduleFromDoctorResponse.FreeSchedules = doctorAvailableSchedules;
        return getScheduleFromDoctorResponse;

    }
}
