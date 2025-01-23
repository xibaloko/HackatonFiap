using FluentValidation;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;

namespace HackatonFiap.Identity.Application.UseCases.CreateAccount;

public sealed class CreateAccountRequestValidator : RequestValidator<CreateAccountRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Username is required.");

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("E-mail is required.")
            .EmailAddress()
            .WithMessage("Inform a valid e-mail address");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(request => request.ConfirmationPassword)
           .NotEmpty()
           .WithMessage("Confirmation Password is required.")
           .Equal(request => request.Password)
           .WithMessage("Confirmation Password must match Password.");

    }
}
