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

        RuleFor(request => request.Role)
            .NotEmpty()
            .WithMessage("Role is required.")
            .Must(value => new[] { "Admin", "Doctor", "Patient" }.Contains(value))
            .WithMessage("Role is invalid. Allowed roles are Admin, Doctor, or Patient.");
    }
}
