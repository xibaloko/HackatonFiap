using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientValidator : RequestValidator<UpdatePatientRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");

        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required");

        RuleFor(request => request.Email)
            .EmailAddress()
            .WithMessage("E-mail is required.");

        RuleFor(request => request.CPF)
            .NotEmpty()
            .WithMessage("CPF is required.")
            .Length(11)
            .WithMessage("Invalid CPF.");

        RuleFor(request => request.RG)
            .NotEmpty()
            .WithMessage("RG is required.");
    }
}
