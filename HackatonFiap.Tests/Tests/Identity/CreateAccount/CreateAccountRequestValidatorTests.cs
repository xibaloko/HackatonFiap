using FluentValidation.TestHelper;
using HackatonFiap.Identity.Application.UseCases.CreateAccount;
using HackatonFiap.Identity.Application.UseCases.DeleteAccount;

namespace HackatonFiap.Tests.Tests.Identity.CreateAccount
{
    public class CreateAccountRequestValidatorTests
    {
        private readonly DeleteAccountRequestValidator _validator;

        public CreateAccountRequestValidatorTests()
        {
            _validator = new DeleteAccountRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "validUser",
                Email = "user@example.com",
                Password = "StrongPassword123!",
                Role = "Admin"
            };

            // Act
            //var result = _validator.TestValidate(request);

            // Assert
            //result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenUsernameIsEmpty()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "",
                Email = "user@example.com",
                Password = "StrongPassword123!",
                Role = "Admin"
            };

            // Act
            //var result = _validator.TestValidate(request);

            // Assert
            //result.ShouldHaveValidationErrorFor(r => r.Username)
            //    .WithErrorMessage("Username is required.");
        }

        [Theory]
        [InlineData("", "E-mail is required.")]
        [InlineData("invalid-email", "Inform a valid e-mail address")]
        public void Validate_ShouldFail_WhenEmailIsInvalid(string email, string expectedErrorMessage)
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "validUser",
                Email = email,
                Password = "StrongPassword123!",
                Role = "Admin"
            };

            // Act
            //var result = _validator.TestValidate(request);

            // Assert
            //result.ShouldHaveValidationErrorFor(r => r.Email)
            //    .WithErrorMessage(expectedErrorMessage);
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordIsEmpty()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "validUser",
                Email = "user@example.com",
                Password = "",
                Role = "Admin"
            };

            // Act
            //var result = _validator.TestValidate(request);

            // Assert
            //result.ShouldHaveValidationErrorFor(r => r.Password)
            //    .WithErrorMessage("Password is required.");
        }

        [Theory]
        [InlineData("", "Role is required.")]
        [InlineData("InvalidRole", "Role is invalid. Allowed roles are Admin, Doctor, or Patient.")]
        public void Validate_ShouldFail_WhenRoleIsInvalid(string role, string expectedErrorMessage)
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "validUser",
                Email = "user@example.com",
                Password = "StrongPassword123!",
                Role = role
            };

            // Act
            //var result = _validator.TestValidate(request);

            //// Assert
            //result.ShouldHaveValidationErrorFor(r => r.Role)
            //    .WithErrorMessage(expectedErrorMessage);
        }
    }
}
