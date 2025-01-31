using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

namespace HackatonFiap.Tests.Tests.Doctors.GetDoctorByUuid
{
    public class GetDoctorByUuidRequestValidatorTests
    {
        private readonly GetDoctorByUuidRequestValidator _validator;

        public GetDoctorByUuidRequestValidatorTests()
        {
            _validator = new GetDoctorByUuidRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenUuidIsValid()
        {
            // Arrange
            var request = new GetDoctorByUuidRequest(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r.Uuid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenUuidIsEmpty()
        {
            // Arrange
            var request = new GetDoctorByUuidRequest(Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Uuid)
                .WithErrorMessage("Uuid is required.");
        }
    }
}