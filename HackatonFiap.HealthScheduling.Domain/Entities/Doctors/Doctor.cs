using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

public sealed class Doctor : EntityBase
{
    public string Name { get; private set; }
    public string LastName { get; private set; }

    public Doctor(string name, string lastName)
    {
        Name = name;
        LastName = lastName;
    }
}
