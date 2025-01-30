using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Agendas;
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

        return response;
    }

    private static GetScheduleFromDoctorResponse CreateResponse(GetScheduleFromDoctorRequest request, IEnumerable<Schedule> schedules)
    {

        GetScheduleFromDoctorResponse getScheduleFromDoctorResponse = new GetScheduleFromDoctorResponse
        {
            DoctorUuid = request.DoctorUuId
        };
        List<DoctorAvailableSchedule> doctorAvailableSchedules = new List<DoctorAvailableSchedule>();
        foreach (var schedule in schedules)
        {
            var doctorAvailableSchedule = new DoctorAvailableSchedule
            {
                DateSchedule = DateOnly.Parse(schedule.DateHour.Date.ToShortDateString()),
                Hours = new List<TimeOnly> { TimeOnly.Parse(schedule.DateHour.TimeOfDay.ToString()) },
                ScheduleUuid = schedule.Uuid
            };
            doctorAvailableSchedules.Add(doctorAvailableSchedule);
        }
        getScheduleFromDoctorResponse.FreeSchedules = doctorAvailableSchedules;
        return getScheduleFromDoctorResponse;

    }
}
