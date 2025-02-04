using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

namespace HackatonFiap.Tests.Tests.Patients.DeletePatient
{
    public class DeletePatientRequestValidatorTests
    {
        private readonly DeletePatientRequestValidator _validator;

        public DeletePatientRequestValidatorTests()
        {
            _validator = new DeletePatientRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenUuidIsValid()
        {
            // Arrange
            var request = new DeletePatientRequest(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenUuidIsEmpty()
        {
            // Arrange
            var request = new DeletePatientRequest(Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Uuid)
                .WithErrorMessage("Uuid is required.");
        }
    }
}