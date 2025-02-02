using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;

namespace HackatonFiap.Tests.Tests.Doctors.UpdateDoctor
{
    public class UpdateDoctorValidatorTests
    {
        private readonly UpdateDoctorValidator _validator;

        public UpdateDoctorValidatorTests()
        {
            _validator = new UpdateDoctorValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new UpdateDoctorRequest(
                Guid.NewGuid(),
                "John",
                "Doe",
                "john.doe@example.com",
                "12345678900",
                "CRM123456"
            );

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Doe", "john.doe@example.com", "12345678900", "CRM123456", "Name is required.")]
        [InlineData("John", "", "john.doe@example.com", "12345678900", "CRM123456", "Last name is required")]
        [InlineData("John", "Doe", "invalid-email", "12345678900", "CRM123456", "E-mail is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "", "CRM123456", "CPF is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345", "CRM123456", "Invalid CPF.")]
        [InlineData("John", "Doe", "john.doe@example.com", "12345678900", "", "CRM is required.")]
        public void Validate_ShouldFail_WhenFieldIsInvalid(
            string name, string lastName, string email, string cpf, string crm, string expectedErrorMessage)
        {
            // Arrange
            var request = new UpdateDoctorRequest(
                Guid.NewGuid(),
                name,
                lastName,
                email,
                cpf,
                crm
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
            var request = new UpdateDoctorRequest(
                Guid.Empty,
                "John",
                "Doe",
                "john.doe@example.com",
                "12345678900",
                "CRM123456"
            );

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Uuid)
                .WithErrorMessage("Uuid is required.");
        }
    }
}
