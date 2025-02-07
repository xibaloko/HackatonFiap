using System.Linq.Expressions;
using System.Security.Claims;
using FluentAssertions;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using System.Reflection;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.Tests.Tests.Doctors.UpdateDoctor;

public class UpdateDoctorRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly UpdateDoctorRequestHandler _handler;
    private readonly ExeptionHandling _exeptionHandling;

    public UpdateDoctorRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _exeptionHandling = new ExeptionHandling();

        _handler = new UpdateDoctorRequestHandler(
            _repositoriesMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    private Doctor CreateValidDoctor()
    {
        var identityId = Guid.NewGuid();
        var uuid = Guid.NewGuid();

        var doctor = (Doctor)Activator.CreateInstance(typeof(Doctor), true)!;

        typeof(EntityBase).GetField("<Uuid>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, uuid);

        typeof(Doctor).BaseType?.GetField("<IdentityId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, identityId);

        typeof(Doctor).BaseType?.GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, "Dr. John");

        typeof(Doctor).BaseType?.GetField("<Email>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, "john.doe@example.com");

        return doctor;
    }

    [Fact]
    public async Task Handle_ShouldUpdateDoctor_WhenDoctorExists()
    {
        // Arrange
        var doctor = CreateValidDoctor();
        var identityId = doctor.IdentityId!.Value.ToString();

        _httpContextAccessorMock.SetupUserIdentity(identityId);

        var request = new UpdateDoctorRequest(doctor.Uuid, "John Updated", "Doe Updated", "updated.email@example.com", "98765432100", "CRM654321");

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()));

        _repositoriesMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Once);
        _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
    {
        // Arrange
        var identityId = Guid.NewGuid().ToString();
        _httpContextAccessorMock.SetupUserIdentity(identityId);

        var request = new UpdateDoctorRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "doctor not found.");
        _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Never);
        _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUnauthorized()
    {
        // Arrange
        var doctor = CreateValidDoctor();
        var differentIdentityId = Guid.NewGuid().ToString();

        _httpContextAccessorMock.SetupUserIdentity(differentIdentityId);

        var request = new UpdateDoctorRequest(doctor.Uuid, "John Updated", "Doe Updated", "updated.email@example.com", "98765432100", "CRM654321");

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Unauthorized to access the resource.");
        _repositoriesMock.Verify(repo => repo.DoctorRepository.Update(It.IsAny<Doctor>()), Times.Never);
        _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var request = new UpdateDoctorRequest(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");
        
        var identityId = Guid.NewGuid().ToString();
        _httpContextAccessorMock.SetupUserIdentity(identityId);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
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

public static class HttpContextAccessorExtensions
{
    public static void SetupUserIdentity(this Mock<IHttpContextAccessor> httpContextAccessorMock, string userId)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(user);

        httpContextAccessorMock.Setup(h => h.HttpContext).Returns(mockHttpContext.Object);
    }
}
