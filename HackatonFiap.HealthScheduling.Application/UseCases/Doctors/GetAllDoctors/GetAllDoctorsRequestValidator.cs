using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed class GetAllDoctorsRequestValidator : AbstractValidator<GetAllDoctorsRequest>
{
        public GetAllDoctorsRequestValidator()
        {
            // Como não temos parâmetros, não há validações obrigatórias
        }
}
