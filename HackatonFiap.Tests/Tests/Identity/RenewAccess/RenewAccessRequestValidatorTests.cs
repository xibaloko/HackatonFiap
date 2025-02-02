using FluentValidation.TestHelper;
using HackatonFiap.Identity.Application.UseCases.RenewAccess;

namespace HackatonFiap.Tests.Tests.Identity.RenewAccess
{
    public class RenewAccessRequestValidatorTests
    {
        private readonly RenewAccessRequestValidator _validator;

        public RenewAccessRequestValidatorTests()
        {
            _validator = new RenewAccessRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new RenewAccessRequest
            {
                Email = "user@example.com",
                RefreshToken = "valid_refresh_token"
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
            var request = new RenewAccessRequest
            {
                Email = email,
                RefreshToken = "valid_refresh_token"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Email)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Fact]
        public void Validate_ShouldFail_WhenRefreshTokenIsEmpty()
        {
            // Arrange
            var request = new RenewAccessRequest
            {
                Email = "user@example.com",
                RefreshToken = ""
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.RefreshToken)
                .WithErrorMessage("Refresh token is required.");
        }
    }
}
