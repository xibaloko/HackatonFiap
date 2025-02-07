using System.Linq.Expressions;
using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Patients.GetPatientByUuid
{
    public class GetPatientByUuidRequestHandlerTests
    {
        private readonly Mock<IUnitOfWork> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPatientByUuidRequestHandler _handler;

        public GetPatientByUuidRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            //_handler = new GetPatientByUuidRequestHandler(_repositoriesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPatient_WhenPatientExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var patient = new Patient(uuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

            var response = new GetPatientByUuidResponse
            {
                Uuid = uuid,
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                RG = "RG123456"
            };

            _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string?>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mapperMock.Setup(mapper => mapper.Map<GetPatientByUuidResponse>(patient))
                .Returns(response);

            // Act
            var request = new GetPatientByUuidRequest(uuid);
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("John", result.Value.Name);
            Assert.Equal(uuid, result.Value.Uuid);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPatientNotFound()
        {
            // Arrange
            _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string?>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null!);

            // Act
            var request = new GetPatientByUuidRequest(Guid.NewGuid());
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Patient not found.", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            var exeptionHandling = new ExeptionHandling();
            
            // Arrange
            _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string?>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var request = new GetPatientByUuidRequest(Guid.NewGuid());
            
            var result = await exeptionHandling.ExecuteWithExceptionHandling(() => _handler.Handle(request, CancellationToken.None));


            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Database error", result.Errors.Select(e => e.Message));
        }
    }
}
