using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

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
            .WithMessage("Email is required.");

        RuleFor(request => request.CPF)
            .NotEmpty()
            .WithMessage("CPF is required.");

        RuleFor(request => request.RG)
            .NotEmpty()
            .WithMessage("RG is required.");
    }
}
