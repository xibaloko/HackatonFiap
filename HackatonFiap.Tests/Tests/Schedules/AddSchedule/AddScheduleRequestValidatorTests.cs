using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

namespace HackatonFiap.Tests.Tests.Schedules.AddSchedule
{
    public class AddScheduleRequestValidatorTests
    {
        private readonly GenerateTimeSlotsRequestValidator _validator;

        public AddScheduleRequestValidatorTests()
        {
            _validator = new GenerateTimeSlotsRequestValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new GenerateTimeSlotsRequest
            {
                DoctorUuid = Guid.NewGuid(),
                Date = new DateOnly(2024, 6, 1),
                InitialHour = new TimeOnly(8, 0),
                FinalHour = new TimeOnly(10, 0),
                Duration = 30,
                //Avaliable = true
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("0001-01-01", "08:00", 30, "Date is required.")]
        [InlineData("2024-06-01", "", 30, "Initial hours is required.")]
        [InlineData("2024-06-01", "08:00", 0, "Duration is required.")]
        public void Validate_ShouldFail_WhenFieldIsMissing(
            string date, string initialHour, int duration, string expectedErrorMessage)
        {
            // Arrange
            var request = new GenerateTimeSlotsRequest
            {
                DoctorUuid = Guid.NewGuid(),
                Date = string.IsNullOrEmpty(date) ? default : DateOnly.Parse(date),
                InitialHour = string.IsNullOrEmpty(initialHour) ? default : TimeOnly.Parse(initialHour),
                FinalHour = new TimeOnly(10, 0),
                Duration = duration,
                //Avaliable = true
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            Assert.Contains(expectedErrorMessage, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
