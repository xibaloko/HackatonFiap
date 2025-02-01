using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class CreateAppointmentValidator : RequestValidator<CreateAppointmentRequest>
{
    protected override void Validate()
    {
        RuleFor(x => x.PatientUuid)
            .NotEmpty()
            .WithMessage("PatientUuid is required!");

        RuleFor(x => x.ScheduleUuid)
            .NotEmpty()
            .WithMessage("ScheduleUuid is required!");
    }
}
