using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;

public sealed class DeleteScheduleRequestValidator : RequestValidator<DeleteScheduleRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid)
           .NotEmpty()
           .WithMessage("Uuid Schedule is required.");       

    }
}