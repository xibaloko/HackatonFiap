using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed class GetDoctorByUuidRequestValidator : RequestValidator<GetDoctorByUuidRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Uuid is required.");


    }
}
