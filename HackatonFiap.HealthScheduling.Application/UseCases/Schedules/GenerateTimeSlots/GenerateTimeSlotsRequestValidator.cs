using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

public sealed class GenerateTimeSlotsRequestValidator : RequestValidator<GenerateTimeSlotsRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Date)
           .NotEmpty()
           .WithMessage("Date is required.");

        RuleFor(request => request.InitialHour)
           .NotEmpty()
           .WithMessage("Initial hour is required.");

        RuleFor(request => request.FinalHour)
          .NotEmpty()
          .WithMessage("Final hour is required.");
        
        RuleFor(request => request)
            .Must(request => request.InitialHour < request.FinalHour)
            .WithMessage("The initial hour cannot be later than the final hour.");

        RuleFor(request => request.Duration)
           .GreaterThan(0)
           .WithMessage("Duration must be greater than zero.");

        RuleFor(request => request.Price)
           .GreaterThan(0)
           .WithMessage("Price must be greater than zero.");
    }
}
