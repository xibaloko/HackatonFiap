using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

public class DeletePatientRequestValidator : RequestValidator<DeletePatientRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");
    }
}
