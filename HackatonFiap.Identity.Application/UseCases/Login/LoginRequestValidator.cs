using FluentValidation;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;

namespace HackatonFiap.Identity.Application.UseCases.Login;

public sealed class LoginRequestValidator : RequestValidator<LoginRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
