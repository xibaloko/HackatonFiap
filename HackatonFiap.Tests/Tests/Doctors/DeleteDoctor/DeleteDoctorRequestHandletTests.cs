using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;

namespace HackatonFiap.Tests.Tests.Doctors.DeleteDoctor
{
    public class DeleteDoctorRequestValidatorTests
    {
        private readonly DeleteDoctorRequestValidator _validator;

        public DeleteDoctorRequestValidatorTests()
        {
            _validator = new DeleteDoctorRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenUuidIsValid()
        {
            // Arrange
            var request = new DeleteDoctorRequest(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenUuidIsEmpty()
        {
            // Arrange
            var request = new DeleteDoctorRequest(Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Uuid)
                .WithErrorMessage("Uuid is required.");
        }
    }
}