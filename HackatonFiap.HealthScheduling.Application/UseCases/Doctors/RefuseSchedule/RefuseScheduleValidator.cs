using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.RefuseSchedule;

public class RefuseScheduleValidator : RequestValidator<RefuseScheduleRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.ScheduleUuid)
            .NotEmpty()
            .WithMessage("ScheduleUuid is required.");
    }
}
