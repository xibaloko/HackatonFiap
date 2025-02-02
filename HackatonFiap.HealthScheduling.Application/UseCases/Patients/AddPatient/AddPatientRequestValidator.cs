using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class AddPatientRequestValidator : RequestValidator<AddPatientRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(request => request.Email)
          .NotEmpty()
          .WithMessage("E-mail is required.")
          .EmailAddress()
          .WithMessage("Inform a valid e-mail address");

        RuleFor(request => request.CPF)
            .NotEmpty()
            .WithMessage("CPF is required.");

        RuleFor(request => request.RG)
            .NotEmpty()
            .WithMessage("RG is required.");

        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage("Username is required.");

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
