using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Agendas;

public sealed class Schedule : EntityBase
{
    public Schedule()
    {

    }
    public Schedule(
        DateTime dateHour
        , int duration
        ,Doctor doctor)
    {
        Doctor = doctor;
        DateHour = dateHour;
        Duration = duration;
        Avaliable = true;
    }

    public int DoctorId { get; private set; }
    public  Doctor Doctor { get; private set; }
    
    public DateTime DateHour { get; private set; }
    public int Duration { get; private set; }
    public bool Avaliable { get; private set; }
}
