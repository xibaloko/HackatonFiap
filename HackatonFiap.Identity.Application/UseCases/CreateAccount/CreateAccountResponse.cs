namespace HackatonFiap.Identity.Application.UseCases.CreateAccount;

public sealed class CreateAccountResponse
{
    public string IdentityId { get; init; }
    public CreateAccountResponse(string identityId)
    {
        IdentityId = identityId;
    }
}
