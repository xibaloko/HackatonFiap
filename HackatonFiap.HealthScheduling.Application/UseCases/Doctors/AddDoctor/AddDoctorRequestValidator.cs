using FluentValidation;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;
using System.Text.RegularExpressions;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequestValidator : RequestValidator<AddDoctorRequest>
{
    protected override void Validate()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");

        RuleFor(request => request.Email)
          .NotEmpty()
          .WithMessage("E-mail is required.")
          .EmailAddress()
          .WithMessage("Inform a valid e-mail address.");

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

        RuleFor(request => request.CPF)
            .NotEmpty()
            .WithMessage("CPF is required.")
            .Must(IsValidCpf)
            .WithMessage("Invalid CPF.");

        RuleFor(request => request.CRM)
            .NotEmpty()
            .WithMessage("CRM is required.")
            .Must(IsValidCrm)
            .WithMessage("Invalid CPF.");

        RuleFor(request => request.MedicalSpecialtyUuid)
            .NotEmpty()
            .WithMessage("MedicalSpecialty is required.");

        RuleFor(request => request.Role)
           .NotEmpty()
           .WithMessage("Role is required.")
           .Must(value => new[] { "Doctor", "Patient" }.Contains(value))
           .WithMessage("Role is invalid. Allowed roles are Doctor or Patient.");
    }

    private bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = Regex.Replace(cpf, @"\D", "");

        if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = tempCpf.Select((t, i) => (t - '0') * multiplier1[i]).Sum();

        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit1;
        sum = tempCpf.Select((t, i) => (t - '0') * multiplier2[i]).Sum();

        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf.EndsWith(digit1.ToString() + digit2.ToString());
    }

    public static bool IsValidCrm(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm)) return false;

        string pattern = @"^\d{1,6}-[A-Z]{2}$";

        return Regex.IsMatch(crm, pattern);
    }
}

