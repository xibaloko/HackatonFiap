using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

public sealed class Doctor : UserIdentity
{
    public string CRM { get; private set; }
    public int? MedicalSpecialtyId { get; private set; }
    public MedicalSpecialty? MedicalSpecialty { get; private set; }

#nullable disable
    public Doctor()
    {
    }
    #nullable enable

    public Doctor(Guid identityId, string name, string lastName, string email, string cpf, string crm)
        : base(identityId, name, lastName, email, cpf)
    {
        CRM = crm;
    }

    public void UpdateBasicInformations(string name, string lastName, string email, string cpf, string crm)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        CPF = cpf;
        CRM = crm;
    }
}

