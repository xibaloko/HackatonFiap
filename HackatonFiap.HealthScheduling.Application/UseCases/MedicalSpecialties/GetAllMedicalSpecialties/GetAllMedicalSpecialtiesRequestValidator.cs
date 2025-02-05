using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public sealed class GetAllMedicalSpecialtiesRequestValidator : RequestValidator<GetAllMedicalSpecialtiesRequest>
{
    protected override void Validate()
    {
    }
}
