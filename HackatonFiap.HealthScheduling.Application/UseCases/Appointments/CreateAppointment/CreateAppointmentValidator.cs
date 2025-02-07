using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.CreateAppointment;

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
