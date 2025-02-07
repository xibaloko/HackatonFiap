using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetDoctorAppointments;

public class GetDoctorAppointmentsValidator : RequestValidator<GetDoctorAppointmentsRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
            .NotEmpty()
            .WithMessage("Doctor Uuid is required.");
    }
}