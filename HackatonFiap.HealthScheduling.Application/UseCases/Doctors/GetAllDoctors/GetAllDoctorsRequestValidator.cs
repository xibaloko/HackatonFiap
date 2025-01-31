using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed class GetAllDoctorsRequestValidator : RequestValidator<GetAllDoctorsRequest>
{
    protected override void Validate()
    {
    }
}
