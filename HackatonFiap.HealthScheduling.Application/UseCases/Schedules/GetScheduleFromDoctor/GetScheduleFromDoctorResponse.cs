namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

public class GetScheduleFromDoctorResponse
{
    public Guid DoctorUuid { get; set; }
    public List<DoctorAvailableSchedule> FreeSchedules { get; set; } = [];
}

public class DoctorAvailableSchedule
{
    public DateOnly DateSchedule { get; set; }
    public List<AppointmentResponse> Appointments { get; set; } = [];
}

public class AppointmentResponse
{
    public TimeOnly Hour { get; set; }
    public Guid ScheduleUuid { get; set; }

}