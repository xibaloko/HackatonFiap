using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientById
{
    public class GetPatientByIdRequestValidator : AbstractValidator<GetPatientByIdRequest>
    {
        public GetPatientByIdRequestValidator()
        {
            //RuleFor(x => x.Id)
                //.NotEmpty().WithMessage("O ID do paciente é obrigatório.")
                //.NotEqual(Guid.Empty).WithMessage("O ID do paciente não pode ser vazio.");
        }
    }
}
