using FluentValidation;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;

namespace HackatonFiap.Identity.Application.UseCases.RenewAccess;

public sealed class RenewAccessRequestValidator : RequestValidator<RenewAccessRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage("E-mail is required.")
            .EmailAddress()
            .WithMessage("Inform a valid e-mail.");

        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
