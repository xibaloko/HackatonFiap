using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;

public sealed class MedicalSpecialty : EntityBase
{
    public required string Description { get; init; }

    #nullable disable
    public MedicalSpecialty()
    {
    }
    #nullable enable

    public MedicalSpecialty(string description)
    {
        Description = description;
    }
}
