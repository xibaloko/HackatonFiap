using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Patients;

public sealed class Patient : UserIdentity
{
    public string RG { get; private set; }

    #nullable disable
    public Patient()
    {
    }
    #nullable enable
    public Patient(string rG)
    {
        RG = rG;
    }
}
