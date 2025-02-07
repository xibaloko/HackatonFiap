using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetPatientAppointments;

public class GetPatientAppointmentsValidator : RequestValidator<GetPatientAppointmentsRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Patient Uuid is required.");
    }
}