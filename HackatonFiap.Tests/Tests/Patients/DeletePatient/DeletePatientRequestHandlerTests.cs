using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace HackatonFiap.Tests.Tests.Patients.DeletePatient;

public class DeletePatientRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly DeletePatientRequestHandler _handler;
    private readonly ExeptionHandling _exeptionHandling;

    public DeletePatientRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _handler = new DeletePatientRequestHandler(_repositoriesMock.Object, _httpContextAccessorMock.Object);
        _exeptionHandling = new ExeptionHandling();
    }

    private void SetupUserContext(Guid identityId)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, identityId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(user);

        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(mockHttpContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeletePatient_WhenPatientExists()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var identityId = Guid.NewGuid();
        var patient = new Patient(identityId, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

        SetupUserContext(identityId);

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
        var result = await _handler.Handle(new DeletePatientRequest(uuid), CancellationToken.None);

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
        SetupUserContext(Guid.NewGuid());

        _repositoriesMock
            .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient)null!);

        // Act
        var result = await _handler.Handle(new DeletePatientRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Patient not found.", result.Errors.Select(e => e.Message));
        _repositoriesMock.Verify(repo => repo.PatientRepository.Update(It.IsAny<Patient>()), Times.Never);
        _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null!);

        // Act
        var result = await _handler.Handle(new DeletePatientRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Unauthorized: User not found", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotOwner()
    {
        // Arrange
        var patientIdentity = Guid.NewGuid();
        var differentIdentity = Guid.NewGuid(); // Simulando um usuário diferente autenticado

        SetupUserContext(differentIdentity);

        var patient = new Patient(patientIdentity, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

        _repositoriesMock
            .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await _handler.Handle(new DeletePatientRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Unauthorized to access the resource.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        SetupUserContext(Guid.NewGuid());

        _repositoriesMock
            .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _exeptionHandling.ExecuteWithExceptionHandling(() =>
            _handler.Handle(new DeletePatientRequest(Guid.NewGuid()), CancellationToken.None));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Unexpected error", result.Errors.Select(e => e.Message));
    }
}
