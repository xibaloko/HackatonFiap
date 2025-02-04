using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;

namespace HackatonFiap.Tests.Tests.Schedules.GetScheduleFromDoctor
{
    public class GetScheduleFromDoctorRequestValidatorTests
    {
        private readonly GetScheduleFromDoctorRequestValidator _validator;

        public GetScheduleFromDoctorRequestValidatorTests()
        {
            _validator = new GetScheduleFromDoctorRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenDoctorUuidIsValid()
        {
            // Arrange
            var request = new GetScheduleFromDoctorRequest(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenDoctorUuidIsEmpty()
        {
            // Arrange
            var request = new GetScheduleFromDoctorRequest(Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.DoctorUuId)
                .WithErrorMessage("Doctor is required.");
        }
    }
}