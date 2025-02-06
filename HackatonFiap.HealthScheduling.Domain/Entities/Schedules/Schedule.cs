using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Schedules;

public sealed class Schedule : EntityBase
{
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    public DateTime InitialDateHour { get; private set; }
    public DateTime FinalDateHour { get; private set; }
    public bool Avaliable { get; private set; }
    public decimal MedicalAppointmentPrice { get; private set; }
    public Appointment? Appointment { get; private set; }

#nullable disable
    public Schedule()
    {
    }
#nullable enable

    public Schedule(DateTime initialDateHour, DateTime finalDateTime, Doctor doctor, decimal medicalAppointmentPrice)
    {
        Doctor = doctor;
        InitialDateHour = initialDateHour;
        FinalDateHour = finalDateTime;
        Avaliable = true;
        MedicalAppointmentPrice = medicalAppointmentPrice;
    }

    public void RescheduleAppointment(DateTime initialDateHour, DateTime finalDateTime, decimal medicalAppointmentPrice)
    {
        InitialDateHour = initialDateHour;
        FinalDateHour = finalDateTime;
        MedicalAppointmentPrice = medicalAppointmentPrice;
    }

    public void SetAppointment()
    {
        Avaliable = false;
    }

    public void MakeAppointmentAvailable()
    {
        Avaliable = true;
    }

}
