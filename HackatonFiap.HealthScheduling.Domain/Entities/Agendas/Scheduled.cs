namespace HackatonFiap.HealthScheduling.Domain.Entities.Agendas;

public sealed class Scheduled
{
    public Scheduled()
    {
        
    }
    public Scheduled(int idAgenda, string pacienteId, bool confirmed)
    {
        ScheduleId = idAgenda;
        PatientId = pacienteId;
        Confirmed = confirmed;
    }
    public int ScheduleId { get; set; }
    public string PatientId { get; private set; }
    public bool Confirmed { get; private set; }

}