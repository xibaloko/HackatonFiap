using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;

namespace HackatonFiap.Tests.Tests.Schedules.GenerateTimeSlots;

public class GenerateTimeSlotsRequestValidatorTests
{
    private readonly GenerateTimeSlotsRequestValidator _validator;

    public GenerateTimeSlotsRequestValidatorTests()
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
            Price = 150.00m
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("0001-01-01", "08:00", "10:00", 30, 150, "Date is required.")]
    [InlineData("2024-06-01", "", "10:00", 30, 150, "Initial hour is required.")]
    [InlineData("2024-06-01", "08:00", "", 30, 150, "Final hour is required.")]
    [InlineData("2024-06-01", "10:00", "08:00", 30, 150, "The initial hour cannot be later than the final hour.")]
    [InlineData("2024-06-01", "08:00", "10:00", 0, 150, "Duration must be greater than zero.")]
    [InlineData("2024-06-01", "08:00", "10:00", 30, 0, "Price must be greater than zero.")]
    public void Validate_ShouldFail_WhenFieldIsInvalid(
        string date, string initialHour, string finalHour, int duration, decimal price, string expectedErrorMessage)
    {
        // Arrange
        var request = new GenerateTimeSlotsRequest
        {
            DoctorUuid = Guid.NewGuid(),
            Date = string.IsNullOrEmpty(date) ? default : DateOnly.Parse(date),
            InitialHour = string.IsNullOrEmpty(initialHour) ? default : TimeOnly.Parse(initialHour),
            FinalHour = string.IsNullOrEmpty(finalHour) ? default : TimeOnly.Parse(finalHour),
            Duration = duration,
            Price = price
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        var errorMessages = result.Errors.Select(e => e.ErrorMessage);
        Assert.Contains(expectedErrorMessage, errorMessages);
    }
}
