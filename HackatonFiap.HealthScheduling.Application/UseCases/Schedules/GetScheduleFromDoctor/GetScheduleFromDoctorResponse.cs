namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

public class GetScheduleFromDoctorResponse
{
    public Guid DoctorUuid { get; set; }
    public List<DoctorAvailableSchedule> FreeSchedules { get; set; } = [];
}


public class DoctorAvailableSchedule
{
    public DateOnly DateSchedule { get; set; }
    public List<Appointment> Appointments { get; set; } = [];
}

public class Appointment
{
    public TimeOnly Hour { get; set; }
    public Guid ScheduleUuid { get; set; }

}