using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using System.Text;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

public sealed class Doctor : UserIdentity
{
    public Doctor()
    {
            
    }
    public Doctor(string crm)
    {
        CRM = crm;
    }
    public string CRM { get; private set; }
}

