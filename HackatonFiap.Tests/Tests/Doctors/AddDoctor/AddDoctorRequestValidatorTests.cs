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
                CPF = "12345678900",
                CRM = "CRM123456"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null, "LastName", "Email", "12345678900", "CRM123456", "Name is required.")]
        [InlineData("John", null, "Email", "12345678900", "CRM123456", "Last name is required.")]
        [InlineData("John", "Doe", null, "12345678900", "CRM123456", "Email is required.")]
        [InlineData("John", "Doe", "Email", null, "CRM123456", "CPF is required.")]
        [InlineData("John", "Doe", "Email", "12345678900", null, "CRM is required.")]
        public void Validate_ShouldFail_WhenARequiredFieldIsMissing(
            string name, string lastName, string email, string cpf, string crm, string expectedErrorMessage)
        {
            // Arrange
            var request = new AddDoctorRequest
            {
                Name = name,
                LastName = lastName,
                Email = email,
                CPF = cpf,
                CRM = crm
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.Contains(expectedErrorMessage, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
