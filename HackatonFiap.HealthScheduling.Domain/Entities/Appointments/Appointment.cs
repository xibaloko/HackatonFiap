using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Appointments;

public class Appointment : EntityBase
{
    public int ScheduleId { get; init; }
    public int PatientId { get; init; }
    public Patient Patient { get; init; }
    public Schedule Schedule { get; init; }
    public bool IsCanceledByPatient { get; private set; }
    public string? CancellationReason { get; private set; }

    #nullable disable
    public Appointment()
    {
    }
    #nullable enable

    public Appointment(Patient patient, Schedule schedule)
    {
        Patient = patient;
        Schedule = schedule;
        IsCanceledByPatient = false;
    }

    public void SetCancellation(string reason) 
    {
        IsCanceledByPatient = true;
        CancellationReason = reason;
        Schedule.MakeAppointmentAvailable();
    }
}
