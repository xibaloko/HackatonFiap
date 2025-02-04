using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Patients.UpdatePatient
{
    public class UpdatePatientRequestHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly UpdatePatientRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public UpdatePatientRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            _handler = new UpdatePatientRequestHandler(_repositoriesMock.Object);
            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldUpdatePatient_WhenPatientExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var patient = new Patient(uuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

            var request = new UpdatePatientRequest(uuid, "John Updated", "Doe Updated", "updated.email@example.com", "98765432100", "RG654321");

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.Update(It.IsAny<Patient>()));

            _repositoriesMock
                .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _repositoriesMock.Verify(repo => repo.PatientRepository.Update(It.IsAny<Patient>()), Times.Once);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPatientNotFound()
        {
            // Arrange
            var request = new UpdatePatientRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Patient not found.", result.Errors.Select(e => e.Message));
            _repositoriesMock.Verify(repo => repo.PatientRepository.Update(It.IsAny<Patient>()), Times.Never);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new UpdatePatientRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _exeptionHandling.ExecuteWithExceptionHandling(() =>
                _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Unexpected error", result.Errors.Select(e => e.Message));
        }
    }
}
