using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.UseCases.Login;

public sealed class LoginRequest : IRequest<Result<LoginResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
