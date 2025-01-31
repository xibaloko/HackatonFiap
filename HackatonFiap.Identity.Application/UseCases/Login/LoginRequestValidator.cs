using FluentValidation;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;

namespace HackatonFiap.Identity.Application.UseCases.Login;

public sealed class LoginRequestValidator : RequestValidator<LoginRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("E-mail is required.")
            .EmailAddress()
            .WithMessage("Inform a valid e-mail.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
