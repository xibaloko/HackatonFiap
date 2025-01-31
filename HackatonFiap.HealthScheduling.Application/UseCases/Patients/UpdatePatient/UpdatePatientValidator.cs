using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientValidator : RequestValidator<UpdatePatientRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Uuid).NotEmpty().WithMessage("O UUID do paciente é obrigatório.");
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(request => request.LastName).NotEmpty().WithMessage("O sobrenome é obrigatório.");
        RuleFor(request => request.Email).EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(request => request.CPF).NotEmpty().Length(11).WithMessage("CPF inválido.");
        RuleFor(request => request.RG).NotEmpty().WithMessage("RG é obrigatório.");
    }
}
