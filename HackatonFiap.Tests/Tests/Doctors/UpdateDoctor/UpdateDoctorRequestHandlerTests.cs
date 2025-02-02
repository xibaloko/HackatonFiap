using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Doctors.UpdateDoctor
{
    public class UpdateDoctorRequestHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly UpdateDoctorRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public UpdateDoctorRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            _handler = new UpdateDoctorRequestHandler(_repositoriesMock.Object);
            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldUpdateDoctor_WhenDoctorExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var doctor = new Doctor(uuid, "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            var request = new UpdateDoctorRequest(uuid, "John Updated", "Doe Updated", "updated.email@example.com", "98765432100", "CRM654321");

            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctor);

            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()));

            _repositoriesMock
                .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Once);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
        {
            // Arrange
            var request = new UpdateDoctorRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Doctor)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("doctor not found.", result.Errors.Select(e => e.Message));
            _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Never);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new UpdateDoctorRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
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
