using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddAppointmentSlot;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

public sealed class AddAppointmentSlotRequestValidator : RequestValidator<AddAppointmentSlotRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Date)
           .NotEmpty()
           .WithMessage("Date is required.");

        RuleFor(request => request.InitialHour)
           .NotEmpty()
           .WithMessage("Initial hours is required.");

        RuleFor(request => request.Duration)
           .NotEmpty()
           .WithMessage("Duration is required.");

        RuleFor(request => request.Price)
           .NotEmpty()
           .WithMessage("Price is required.");

    }
}