using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

namespace HackatonFiap.Tests.Tests.Patients.UpdatePatient
{
    public class UpdatePatientValidatorTests
    {
        private readonly UpdatePatientValidator _validator;

        public UpdatePatientValidatorTests()
        {
            _validator = new UpdatePatientValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new UpdatePatientRequest(
                Guid.NewGuid(),
                "John",
                "Doe",
                "john.doe@example.com",
                "12345678900",
                "RG123456"
            );

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Doe", "john.doe@example.com", "12345678900", "RG123456", "Name is required.")]
        [InlineData("John", "", "john.doe@example.com", "12345678900", "RG123456", "Last name is required")]
        [InlineData("John", "Doe", "invalid-email", "12345678900", "RG123456", "E-mail is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "", "RG123456", "CPF is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345", "RG123456", "Invalid CPF.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678900", "", "RG is required.")]
        public void Validate_ShouldFail_WhenFieldIsInvalid(
            string name, string lastName, string email, string cpf, string rg, string expectedErrorMessage)
        {
            // Arrange
            var request = new UpdatePatientRequest(
                Guid.NewGuid(),
                name,
                lastName,
                email,
                cpf,
                rg
            );

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.Contains(expectedErrorMessage, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Validate_ShouldFail_WhenUuidIsEmpty()
        {
            // Arrange
            var request = new UpdatePatientRequest(
                Guid.Empty,
                "John",
                "Doe",
                "john.doe@example.com",
                "12345678900",
                "RG123456"
            );

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Uuid)
                .WithErrorMessage("Uuid is required.");
        }
    }
}
