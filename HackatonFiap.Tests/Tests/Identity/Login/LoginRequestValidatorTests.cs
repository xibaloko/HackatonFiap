using FluentValidation.TestHelper;
using HackatonFiap.Identity.Application.UseCases.Login;

namespace HackatonFiap.Tests.Tests.Identity.Login
{
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator;

        public LoginRequestValidatorTests()
        {
            _validator = new LoginRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "user@example.com",
                Password = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "E-mail is required.")]
        [InlineData("invalid-email", "Inform a valid e-mail.")]
        public void Validate_ShouldFail_WhenEmailIsInvalid(string email, string expectedErrorMessage)
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = email,
                Password = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Username)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordIsEmpty()
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "user@example.com",
                Password = ""
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Password)
                .WithErrorMessage("Password is required.");
        }
    }
}