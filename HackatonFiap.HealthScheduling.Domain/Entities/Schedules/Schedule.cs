using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

public sealed class Schedule : EntityBase
{
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    public DateTime InitialDateHour { get; private set; }
    public DateTime FinalDateHour { get; private set; }
    public int Duration { get; private set; }
    public bool Avaliable { get; private set; }
    public decimal MedicalAppointmentPrice { get; private set; }

    #nullable disable
    public Schedule()
    {
    }
    #nullable enable

    public Schedule(DateTime initialDateHour,DateTime finalDateTime, int duration, Doctor doctor, decimal medicalAppointmentPrice)
    {
        Doctor = doctor;
        InitialDateHour = initialDateHour;
        FinalDateHour = finalDateTime;
        Duration = duration;
        Avaliable = true;
        MedicalAppointmentPrice = medicalAppointmentPrice;
    }

    public void SetAppointment()
    {
        Avaliable = false;
    }

}
