using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

public sealed class Doctor : UserIdentity
{
    public string CRM { get; private set; }

    public Doctor()
    {
        
    }

    public Doctor(string crm)
    {
        CRM = crm;
    }
}

