namespace HackatonFiap.HealthScheduling.Domain.Entities.Bases;

public abstract class UserIdentity : EntityBase
{
    public Guid IdentityId { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CPF { get; init; }
}
