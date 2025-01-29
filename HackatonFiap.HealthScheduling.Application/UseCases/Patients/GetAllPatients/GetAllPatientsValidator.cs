using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients
{
    public class GetAllPatientsValidator : AbstractValidator<GetAllPatientsRequest>
    {
        public GetAllPatientsValidator()
        {
            // Como não temos parâmetros, não há validações obrigatórias
        }
    }
}
