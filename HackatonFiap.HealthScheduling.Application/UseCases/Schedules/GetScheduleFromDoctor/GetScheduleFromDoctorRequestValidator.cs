using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor
{
    public sealed class GetScheduleFromDoctorRequestValidator : RequestValidator<GetScheduleFromDoctorRequest>
    {
        protected override void Validate()
        {
            RuleFor(request => request.DoctorUuId)
                .NotEmpty()
                .WithMessage("Doctor is required.");          
        }
    }

}
