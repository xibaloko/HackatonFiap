using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed class AddScheduleRequestValidator : RequestValidator<AddScheduleRequest>
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

    }
}