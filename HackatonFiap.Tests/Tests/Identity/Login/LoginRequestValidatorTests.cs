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
        [InlineData("", "Username is required.")]
        [InlineData(null, "Username is required.")]
        public void Validate_ShouldFail_WhenUsernameIsInvalid(string username, string expectedErrorMessage)
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = username,
                Password = "ValidPassword123!"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Username)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Theory]
        [InlineData("", "Password is required.")]
        [InlineData(null, "Password is required.")]
        public void Validate_ShouldFail_WhenPasswordIsInvalid(string password, string expectedErrorMessage)
        {
            // Arrange
            var request = new LoginRequest
            {
                Username = "user@example.com",
                Password = password
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Password)
                .WithErrorMessage(expectedErrorMessage);
        }
    }
}
