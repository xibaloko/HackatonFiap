using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using System.Reflection;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.Tests.Tests.Doctors.GetDoctorByUuid;

public class GetDoctorByUuidRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly GetDoctorByUuidRequestHandler _handler;

    public GetDoctorByUuidRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _handler = new GetDoctorByUuidRequestHandler(
            _repositoriesMock.Object,
            _mapperMock.Object,
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

        typeof(Doctor).BaseType?.GetField("<IdentityId>k__BackingField",
                BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, identityId);

        typeof(Doctor).BaseType?.GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, "Dr. John");

        typeof(Doctor).BaseType?.GetField("<Email>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, "john.doe@example.com");

        return doctor;
    }

    [Fact]
    public async Task Handle_ShouldReturnDoctor_WhenDoctorExists()
    {
        // Arrange
        var doctor = CreateValidDoctor();
        var identityId = doctor.IdentityId!.Value.ToString();

        _httpContextAccessorMock.SetupUserIdentity(identityId);

        var response = new GetDoctorByUuidResponse
        {
            Uuid = doctor.Uuid,
            Name = "Dr. John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            CPF = "12345678900",
            CRM = "CRM123456"
        };

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        _mapperMock.Setup(mapper => mapper.Map<GetDoctorByUuidResponse>(doctor))
            .Returns(response);

        // Act
        var request = new GetDoctorByUuidRequest(doctor.Uuid);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Dr. John");
        result.Value.Uuid.Should().Be(doctor.Uuid);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
    {
        // Arrange
        var identityId = Guid.NewGuid().ToString();
        _httpContextAccessorMock.SetupUserIdentity(identityId);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor)null!);

        // Act
        var request = new GetDoctorByUuidRequest(Guid.NewGuid());
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Doctor not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserUnauthorized()
    {
        // Arrange
        var doctor = CreateValidDoctor();
        var differentIdentityId = Guid.NewGuid().ToString();

        _httpContextAccessorMock.SetupUserIdentity(differentIdentityId);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        // Act
        var request = new GetDoctorByUuidRequest(doctor.Uuid);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Unauthorized to access the resource.");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        var identityId = Guid.NewGuid().ToString();
        _httpContextAccessorMock.SetupUserIdentity(identityId);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await _handler.Handle(new GetDoctorByUuidRequest(Guid.NewGuid()), CancellationToken.None));

        exception.Message.Should().Be("Database error");
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