using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HackatonFiap.Tests.Tests.Schedules.GenerateTimeSlots;

public class GenerateTimeSlotsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly GenerateTimeSlotsRequestHandler _handler;
    private readonly ExeptionHandling _exeptionHandling;

    public GenerateTimeSlotsRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _handler = new GenerateTimeSlotsRequestHandler(_httpContextAccessorMock.Object, _repositoriesMock.Object);
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
    public async Task Handle_ShouldGenerateTimeSlots_WhenDoctorExistsAndNoConflict()
    {
        // Arrange
        var doctorUuid = Guid.NewGuid();
        var identityId = Guid.NewGuid();
        var doctor = new Doctor(identityId, "Dr. John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

        SetupUserContext(identityId);

        var request = new GenerateTimeSlotsRequest
        {
            DoctorUuid = doctorUuid,
            Date = new DateOnly(2024, 6, 1),
            InitialHour = new TimeOnly(8, 0),
            FinalHour = new TimeOnly(10, 0),
            Duration = 30,
            Price = 150.00m
        };

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        _repositoriesMock.Setup(repo => repo.ScheduleRepository.GetAllAsync(
            It.IsAny<Expression<Func<Schedule, bool>>>(),
            It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Schedule>());

        _repositoriesMock.Setup(repo => repo.ScheduleRepository.AddBulkAsync(It.IsAny<IEnumerable<Schedule>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoriesMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        _repositoriesMock.Verify(repo => repo.ScheduleRepository.AddBulkAsync(It.IsAny<IEnumerable<Schedule>>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
    {
        // Arrange
        SetupUserContext(Guid.NewGuid());

        var request = new GenerateTimeSlotsRequest
        {
            DoctorUuid = Guid.NewGuid(),
            Date = new DateOnly(2024, 6, 1),
            InitialHour = new TimeOnly(8, 0),
            FinalHour = new TimeOnly(10, 0),
            Duration = 30,
            Price = 150.00m
        };

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
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
        Assert.Contains("Doctor not found!", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSchedulesConflict()
    {
        // Arrange
        var doctorUuid = Guid.NewGuid();
        var identityId = Guid.NewGuid();
        var doctor = new Doctor(identityId, "Dr. John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

        SetupUserContext(identityId);

        var request = new GenerateTimeSlotsRequest
        {
            DoctorUuid = doctorUuid,
            Date = new DateOnly(2024, 6, 1),
            InitialHour = new TimeOnly(8, 0),
            FinalHour = new TimeOnly(10, 0),
            Duration = 30,
            Price = 150.00m
        };

        var conflictingSchedules = new List<Schedule>
        {
            new Schedule(new DateTime(2024, 6, 1, 8, 30, 0), new DateTime(2024, 6, 1, 9, 0, 0), doctor, 150.00m)
        };

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        _repositoriesMock.Setup(repo => repo.ScheduleRepository.GetAllAsync(
            It.IsAny<Expression<Func<Schedule, bool>>>(),
            It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictingSchedules);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Exists Schedules conflicted", result.Errors.Select(e => e.Message).First());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        SetupUserContext(Guid.NewGuid());

        var request = new GenerateTimeSlotsRequest
        {
            DoctorUuid = Guid.NewGuid(),
            Date = new DateOnly(2024, 6, 1),
            InitialHour = new TimeOnly(8, 0),
            FinalHour = new TimeOnly(10, 0),
            Duration = 30,
            Price = 150.00m
        };

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
