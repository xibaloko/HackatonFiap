using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid
{
    public class GetPatientByUuidRequestValidator : AbstractValidator<GetPatientByUuidRequest>
    {
        public GetPatientByUuidRequestValidator()
        {
            RuleFor(request => request.Uuid)
                .NotEmpty()
                .WithMessage("Uuid is required.");
        }
    }
}
