//using System.Linq.Expressions;
//using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;
//using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
//using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
//using HackatonFiap.Tests.Helpers;
//using Moq;

//namespace HackatonFiap.Tests.Tests.Doctors.DeleteDoctor
//{
//    public class DeleteDoctorRequestHandlerTests
//    {
//        private readonly Mock<IUnitOfWork> _repositoriesMock;
//        private readonly DeleteDoctorRequestHandler _handler;

//        public DeleteDoctorRequestHandlerTests()
//        {
//            _repositoriesMock = new Mock<IUnitOfWork>();
//            //_handler = new DeleteDoctorRequestHandler(_repositoriesMock.Object);
//        }

//        [Fact]
//        public async Task Handle_ShouldDeleteDoctor_WhenDoctorExists()
//        {
//            // Arrange
//            var uuid = Guid.NewGuid();
//            var doctor = new Doctor(uuid, "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

//            _repositoriesMock
//                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
//                    It.IsAny<Expression<Func<Doctor, bool>>>(),
//                    It.IsAny<string>(),
//                    It.IsAny<bool>(),
//                    It.IsAny<CancellationToken>()))
//                .ReturnsAsync(doctor);

//            _repositoriesMock
//                .Setup(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()));

//            _repositoriesMock
//                .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
//                .Returns(Task.CompletedTask);

//            // Act
//            var result = await _handler.Handle(new DeleteDoctorRequest(uuid), CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.IsSuccess);
//            _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Once);
//            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
//        }

//        [Fact]
//        public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
//        {
//            // Arrange
//            _repositoriesMock
//                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
//                    It.IsAny<Expression<Func<Doctor, bool>>>(),
//                    It.IsAny<string>(),
//                    It.IsAny<bool>(),
//                    It.IsAny<CancellationToken>()))
//                .ReturnsAsync((Doctor)null!);

//            // Act
//            var result = await _handler.Handle(new DeleteDoctorRequest(Guid.NewGuid()), CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.IsFailed);
//            Assert.Contains("Doctor not found.", result.Errors.Select(e => e.Message));
//            _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Never);
//            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
//        }

//        [Fact]
//        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
//        {
//            var exeptionHandling = new ExeptionHandling();
//            // Arrange
//            _repositoriesMock
//                .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
//                    It.IsAny<Expression<Func<Doctor, bool>>>(),
//                    It.IsAny<string>(),
//                    It.IsAny<bool>(),
//                    It.IsAny<CancellationToken>()))
//                .ThrowsAsync(new Exception("Database error"));

//            // Act
//            var result = await exeptionHandling.ExecuteWithExceptionHandling(() => _handler.Handle(new DeleteDoctorRequest(Guid.NewGuid()), CancellationToken.None));


//            // Assert
//            Assert.NotNull(result);
//            Assert.True(result.IsFailed);
//            Assert.Contains("Database error", result.Errors.Select(e => e.Message));
//        }
//    }
//}
