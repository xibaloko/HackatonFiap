using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

namespace HackatonFiap.Tests.Tests.Doctors.AddDoctor
{
    public class AddDoctorRequestValidatorTests
    {
        private readonly AddDoctorRequestValidator _validator;

        public AddDoctorRequestValidatorTests()
        {
            _validator = new AddDoctorRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new AddDoctorRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678909",
                CRM = "123456-SP",
                Password = "SecurePassword@123",
                Role = "Doctor",
                MedicalSpecialtyUuid = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null, "Doe", "john.doe@example.com", "12345678909", "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "Name is required.")]
        [InlineData("John", null, "john.doe@example.com", "12345678909", "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "Last name is required.")]
        [InlineData("John", "Doe", null, "12345678909", "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "E-mail is required.")]
        [InlineData("John", "Doe", "invalid-email", "12345678909", "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "Inform a valid e-mail address.")]
        [InlineData("John", "Doe", "john.doe@example.com", null, "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "CPF is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "11111111111", "123456-SP", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "Invalid CPF.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678909", null, "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "CRM is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "invalidCRM", "SecurePassword@123", "Doctor", "MedicalSpecialty is required.", "Invalid CRM.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "123456-SP", null, "Doctor", "MedicalSpecialty is required.", "Password is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "123456-SP", "SecurePassword@123", null, "MedicalSpecialty is required.", "Role is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678909", "123456-SP", "SecurePassword@123", "InvalidRole", "MedicalSpecialty is required.", "Role is invalid. Allowed roles are Doctor or Patient.")]
        public void Validate_ShouldFail_WhenARequiredFieldIsMissing(
            string name, string lastName, string email, string cpf, string crm, string password, string role, string expectedErrorMedicalSpecialty, string expectedErrorField)
        {
            // Arrange
            var request = new AddDoctorRequest
            {
                Name = name,
                LastName = lastName,
                Email = email,
                CPF = cpf,
                CRM = crm,
                Password = password,
                Role = role,
                MedicalSpecialtyUuid = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.Contains(expectedErrorField, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(expectedErrorMedicalSpecialty, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
