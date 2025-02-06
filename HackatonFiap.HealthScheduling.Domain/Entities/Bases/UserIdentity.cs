namespace HackatonFiap.HealthScheduling.Domain.Entities.Bases;

public abstract class UserIdentity : EntityBase
{
    public Guid? IdentityId { get; protected set; }
    public string Name { get; protected set; }
    public string LastName { get; protected set; }
    public string Email { get; protected set; }
    public string CPF { get; protected set; }

    #nullable disable
    protected UserIdentity()
    {
        
    }
    #nullable enable

    protected UserIdentity(Guid? identityId, string name, string lastName, string email, string cPF)
    {
        IdentityId = identityId;
        Name = name;
        LastName = lastName;
        Email = email;
        CPF = cPF;
    }

    public void SetIdentityId(Guid identityId) => IdentityId = identityId;
}
