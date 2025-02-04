using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequestValidator : RequestValidator<AddDoctorRequest>
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

        RuleFor(request => request.CRM)
            .NotEmpty()
            .WithMessage("CRM is required.");

        RuleFor(request => request.MedicalSpecialtyUuid)
            .NotEmpty()
            .WithMessage("MedicalSpecialty is required.");
    }
}

