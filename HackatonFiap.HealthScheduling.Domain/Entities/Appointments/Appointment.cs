using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Appointments;

public class Appointment : EntityBase
{
    public Appointment(Patient patient, Schedule schedule)
    {
        Patient = patient;
        Schedule = schedule;
    }
#nullable disable
    public Appointment()
    {

    }
#nullable enable

    public int ScheduleId { get; init; }
    public int PatientId { get; init; }
    public Patient Patient { get; init; }
    public Schedule Schedule { get; init; }


}
