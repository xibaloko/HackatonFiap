//using FluentValidation.TestHelper;
//using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

//namespace HackatonFiap.Tests.Tests.Patients.AddPatient;

//public class AddPatientRequestValidatorTests
//{
//    private readonly AddPatientRequestValidator _validator = new();

//    [Fact]
//    public void Validate_ShouldPass_WhenAllFieldsAreValid()
//    {
//        // Arrange
//        var request = new AddPatientRequest
//        {
//            Username = "john_doe",
//            Password = "SecurePassword123",
//            Role = "Patient",
//            Name = "John",
//            LastName = "Doe",
//            Email = "john.doe@example.com",
//            CPF = "12345678900",
//            RG = "RG123456"
//        };

//        // Act
//        var result = _validator.TestValidate(request);

//        // Assert
//        result.ShouldNotHaveAnyValidationErrors();
//    }

//    [Theory]
//    [InlineData(null, "Doe", "john.doe@example.com", "12345678900", "RG123456", "Username", "Username is required.")]
//    [InlineData("john_doe", null, "john.doe@example.com", "12345678900", "RG123456", "LastName", "Last name is required.")]
//    [InlineData("john_doe", "Doe", null, "12345678900", "RG123456", "Email", "E-mail is required.")]
//    [InlineData("john_doe", "Doe", "invalid_email", "12345678900", "RG123456", "Email", "Inform a valid e-mail address")]
//    [InlineData("john_doe", "Doe", "john.doe@example.com", null, "RG123456", "CPF", "CPF is required.")]
//    [InlineData("john_doe", "Doe", "john.doe@example.com", "12345678900", null, "RG", "RG is required.")]
//    [InlineData("john_doe", "Doe", "john.doe@example.com", "12345678900", "RG123456", null, "Role is required.")]
//    [InlineData("john_doe", "Doe", "john.doe@example.com", "12345678900", "RG123456", "InvalidRole", "Role is invalid. Allowed roles are Admin, Doctor, or Patient.")]
//    public void Validate_ShouldFail_WhenARequiredFieldIsMissing(
//        string username, string lastName, string email, string cpf, string rg, string role, string expectedErrorMessage)
//    {
//        // Arrange
//        var request = new AddPatientRequest
//        {
//            Username = username,
//            Password = "SecurePassword123",
//            Role = role,
//            Name = "John",
//            LastName = lastName,
//            Email = email,
//            CPF = cpf,
//            RG = rg
//        };

//        // Act
//        var result = _validator.TestValidate(request);

//        // Assert
//        Assert.Contains(expectedErrorMessage, result.Errors.Select(e => e.ErrorMessage));
//    }
//}