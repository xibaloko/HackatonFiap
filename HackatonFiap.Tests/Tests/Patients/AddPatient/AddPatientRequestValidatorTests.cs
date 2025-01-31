using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

namespace HackatonFiap.Tests.Tests.Patients.AddPatient
{
    public class AddPatientRequestValidatorTests
    {
        private readonly AddPatientRequestValidator _validator;

        public AddPatientRequestValidatorTests()
        {
            _validator = new AddPatientRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new AddPatientRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                RG = "RG123456"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null, "LastName", "Email", "12345678900", "RG123456", "Name is required.")]
        [InlineData("John", null, "Email", "12345678900", "RG123456", "Last name is required.")]
        [InlineData("John", "Doe", null, "12345678900", "RG123456", "Email is required.")]
        [InlineData("John", "Doe", "Email", null, "RG123456", "CPF is required.")]
        [InlineData("John", "Doe", "Email", "12345678900", null, "RG is required.")]
        public void Validate_ShouldFail_WhenARequiredFieldIsMissing(
            string name, string lastName, string email, string cpf, string rg, string expectedErrorMessage)
        {
            // Arrange
            var request = new AddPatientRequest
            {
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
    }
}