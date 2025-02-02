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

    public Patient(Guid identityId, string name, string lastName, string email, string cpf, string rg)
        : base(identityId, name, lastName, email, cpf)
    {
        RG = rg;
    }

    public void UpdateBasicInformations(string name, string lastName, string email, string cpf, string rg)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        CPF = cpf;
        RG = rg;
    }

    public void SetIdentityId(Guid identityId) => IdentityId = identityId;
}
