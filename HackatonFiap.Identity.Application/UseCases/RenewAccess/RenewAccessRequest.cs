using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.UseCases.RenewAccess;

public sealed class RenewAccessRequest : IRequest<Result<RenewAccessResponse>>
{
    public required string Email { get; init; }
    public required string RefreshToken { get; init; }
}
