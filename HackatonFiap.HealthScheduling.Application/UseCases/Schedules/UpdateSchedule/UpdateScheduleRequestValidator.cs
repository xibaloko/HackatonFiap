using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.UpdateSchedule;

public sealed class UpdateScheduleRequestValidator : RequestValidator<UpdateScheduleRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.InitialHour)
           .NotEmpty()
           .WithMessage("Initial hours is required.");

        RuleFor(request => request.FinalHour)
          .NotEmpty()
          .WithMessage("Final hours is required.");

        RuleFor(request => request)
            .Must(request => request.InitialHour < request.FinalHour)
            .WithMessage("The initial hour can not be higher than the final hour.");

        RuleFor(request => request.Price)
           .NotEmpty()
           .WithMessage("Price is required.");

    }
}