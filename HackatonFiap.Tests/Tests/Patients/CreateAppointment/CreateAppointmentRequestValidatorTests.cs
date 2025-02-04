using FluentValidation.TestHelper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;

namespace HackatonFiap.Tests.Tests.Patients.CreateAppointment
{
    public class CreateAppointmentValidatorTests
    {
        private readonly CreateAppointmentValidator _validator;

        public CreateAppointmentValidatorTests()
        {
            _validator = new CreateAppointmentValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsAreValid()
        {
            // Arrange
            var request = new CreateAppointmentRequest(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ShouldFail_WhenPatientUuidIsEmpty()
        {
            // Arrange
            var request = new CreateAppointmentRequest(Guid.Empty, Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.PatientUuid)
                .WithErrorMessage("PatientUuid is required!");
        }

        [Fact]
        public void Validate_ShouldFail_WhenScheduleUuidIsEmpty()
        {
            // Arrange
            var request = new CreateAppointmentRequest(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.ScheduleUuid)
                .WithErrorMessage("ScheduleUuid is required!");
        }
    }
}