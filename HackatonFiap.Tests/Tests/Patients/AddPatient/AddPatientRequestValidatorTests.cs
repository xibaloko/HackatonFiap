using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

namespace HackatonFiap.Tests.Tests.Patients.AddPatient;

public class AddPatientRequestValidatorTests
{
    private readonly AddPatientRequestValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenAllFieldsAreValid()
    {
        // Arrange
        var request = new AddPatientRequest
        {
            Password = "SecurePassword123",
            Role = "Patient",
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            CPF = "12345678909",
            RG = "RG123456"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Doe", "john.doe@example.com", "12345678909", "RG123456", "Patient", "Name is required.")]
    [InlineData("John", "", "john.doe@example.com", "12345678909", "RG123456", "Patient", "Last name is required.")]
    [InlineData("John", "Doe", "", "12345678909", "RG123456", "Patient", "E-mail is required.")]
    [InlineData("John", "Doe", "invalid_email", "12345678909", "RG123456", "Patient", "Inform a valid e-mail address.")]
    [InlineData("John", "Doe", "john.doe@example.com", "", "RG123456", "Patient", "CPF is required.")]
    [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "", "Patient", "RG is required.")]
    [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "RG123456", "", "Role is required.")]
    [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "RG123456", "InvalidRole", "Role is invalid. Allowed roles are Doctor or Patient.")]
    public void Validate_ShouldFail_WhenARequiredFieldIsMissing(
        string name, string lastName, string email, string cpf, string rg, string role, string expectedErrorMessage)
    {
        // Arrange
        var request = new AddPatientRequest
        {
            Password = "SecurePassword123",
            Role = role,
            Name = name,
            LastName = lastName,
            Email = email,
            CPF = cpf,
            RG = rg
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        Assert.Contains(expectedErrorMessage, result.Errors.Select(e => e.ErrorMessage));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCpfIsInvalid()
    {
        // Arrange
        var request = new AddPatientRequest
        {
            Name = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            CPF = "12345678900", // CPF inválido
            RG = "RG123456",
            Role = "Patient",
            Password = "SecurePassword123"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.CPF)
              .WithErrorMessage("Invalid CPF.");
    }
}
