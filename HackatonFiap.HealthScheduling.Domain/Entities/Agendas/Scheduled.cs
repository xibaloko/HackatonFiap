namespace HackatonFiap.HealthScheduling.Domain.Entities.Agendas;

public sealed class Scheduled
{
    public int ScheduleId { get; private set; }
    public string PatientId { get; private set; }
    public bool Confirmed { get; private set; }

    #nullable disable
    public Scheduled()
    {
    }
    #nullable enable

    public Scheduled(int scheduleId, string pacientId, bool confirmed)
    {
        ScheduleId = scheduleId;
        PatientId = pacientId;
        Confirmed = confirmed;
    }
}