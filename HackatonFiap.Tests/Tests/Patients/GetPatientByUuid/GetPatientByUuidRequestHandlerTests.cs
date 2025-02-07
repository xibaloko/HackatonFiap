using System.Linq.Expressions;
using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace HackatonFiap.Tests.Tests.Patients.GetPatientByUuid;

public class GetPatientByUuidRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly GetPatientByUuidRequestHandler _handler;
    private readonly ExeptionHandling _exeptionHandling;

    public GetPatientByUuidRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _handler = new GetPatientByUuidRequestHandler(_httpContextAccessorMock.Object, _repositoriesMock.Object, _mapperMock.Object);
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
    public async Task Handle_ShouldReturnPatient_WhenPatientExists()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var identityId = Guid.NewGuid();
        var patient = new Patient(identityId, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

        SetupUserContext(identityId);

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
        SetupUserContext(Guid.NewGuid());

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
    public async Task Handle_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null!);

        // Act
        var result = await _handler.Handle(new GetPatientByUuidRequest(Guid.NewGuid()), CancellationToken.None);

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

        _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await _handler.Handle(new GetPatientByUuidRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Unauthorized to access the resource.", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        SetupUserContext(Guid.NewGuid());

        _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _exeptionHandling.ExecuteWithExceptionHandling(() =>
            _handler.Handle(new GetPatientByUuidRequest(Guid.NewGuid()), CancellationToken.None));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Database error", result.Errors.Select(e => e.Message));
    }
}
