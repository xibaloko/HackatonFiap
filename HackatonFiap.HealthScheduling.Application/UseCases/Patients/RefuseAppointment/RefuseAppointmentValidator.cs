using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.RefuseAppointment;

public class RefuseAppointmentValidator : RequestValidator<RefuseAppointmentRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.AppointmentUuid)
            .NotEmpty()
            .WithMessage("Appointment Uuid is required.");

        RuleFor(request => request.CancellationReason)
            .NotEmpty()
            .WithMessage("Cancellation reason is required.");
    }
}
