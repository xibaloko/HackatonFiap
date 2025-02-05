using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseAppointment;

public class RefuseAppointmentValidator : RequestValidator<RefuseAppointmentRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.DoctorUuid)
            .NotEmpty()
            .WithMessage("DoctorUuid is required.");

        RuleFor(request => request.ScheduleUuid)
            .NotEmpty()
            .WithMessage("ScheduleUuid is required.");

        RuleFor(request => request.Avaliable)
            .NotEmpty()
            .WithMessage("Avaliable is required");

    }
}
