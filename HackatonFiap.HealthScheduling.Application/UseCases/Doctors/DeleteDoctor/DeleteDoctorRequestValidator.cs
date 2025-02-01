using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

public class DeleteDoctorRequestValidator : RequestValidator<DeleteDoctorRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");
    }
}
