using FluentValidation;
using HackatonFiap.Identity.Application.Configurations.FluentValidation;


namespace HackatonFiap.Identity.Application.UseCases.DeleteAccount;

public sealed class DeleteAccountRequestValidator : RequestValidator<DeleteAccountRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.IdentityId)
            .NotEmpty()
            .WithMessage("IndentityID is required.");        
      
    }
}
