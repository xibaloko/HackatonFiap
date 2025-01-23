using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.UseCases.CreateAccount;

public sealed class CreateAccountRequest : IRequest<Result>
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmationPassword { get; init; }
}
