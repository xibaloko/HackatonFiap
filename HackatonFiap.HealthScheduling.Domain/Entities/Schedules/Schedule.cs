using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

public sealed class Schedule : EntityBase
{
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    public DateTime DateHour { get; private set; }
    public int Duration { get; private set; }
    public bool Avaliable { get; private set; }


#nullable disable
    public Schedule()
    {
    }
#nullable enable

    public Schedule(DateTime dateHour, int duration, Doctor doctor)
    {
        Doctor = doctor;
        DateHour = dateHour;
        Duration = duration;
        Avaliable = true;
    }

    public void SetAppointment()
    {
        Avaliable = false;
    }

}
