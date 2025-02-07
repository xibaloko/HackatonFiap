using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.UseCases.DeleteAccount;

public sealed class DeleteAccountRequest : IRequest<Result<DeleteAccountResponse>>
{
    public Guid IdentityId { get; set; }
}
