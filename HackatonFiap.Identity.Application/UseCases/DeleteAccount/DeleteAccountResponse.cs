namespace HackatonFiap.Identity.Application.UseCases.DeleteAccount;

public sealed class DeleteAccountResponse
{
    public string IdentityId { get; init; }
    public DeleteAccountResponse(string identityId)
    {
        IdentityId = identityId;
    }
}
