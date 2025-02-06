using System.Linq.Expressions;
using System.Security.Claims;
using AutoFixture;
using FluentAssertions;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using HackatonFiap.Tests.Helpers;
using System.Reflection;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases;

namespace HackatonFiap.Tests.Tests.Patients.CreateAppointment;

public class CreateAppointmentRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _repositoriesMock;
    private readonly Mock<IRabbitMqPublisher> _rabbitMqPublisherMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateAppointmentRequestHandler _handler;
    private readonly Fixture _fixture;

    public CreateAppointmentRequestHandlerTests()
    {
        _repositoriesMock = new Mock<IUnitOfWork>();
        _rabbitMqPublisherMock = new Mock<IRabbitMqPublisher>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _fixture = new Fixture();

        _handler = new CreateAppointmentRequestHandler(
            _repositoriesMock.Object,
            _rabbitMqPublisherMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    private Patient CreateValidPatient()
    {
        var identityId = Guid.NewGuid();
        var patient = (Patient)Activator.CreateInstance(typeof(Patient), true)!;

        typeof(EntityBase).GetField("<Uuid>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(patient, Guid.NewGuid());

        typeof(UserIdentity).GetField("<IdentityId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(patient, identityId);

        typeof(UserIdentity).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(patient, "John Doe");

        typeof(UserIdentity).GetField("<Email>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(patient, "johndoe@example.com");

        return patient;
    }

    private Schedule CreateValidSchedule()
    {
        var schedule = (Schedule)Activator.CreateInstance(typeof(Schedule), true)!;

        typeof(EntityBase).GetField("<Uuid>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(schedule, Guid.NewGuid());

        typeof(Schedule).GetField("<Avaliable>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(schedule, true);

        return schedule;
    }


    private Doctor CreateValidDoctor(int id)
    {
        var doctor = (Doctor)Activator.CreateInstance(typeof(Doctor), true)!;

        typeof(EntityBase).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(doctor, id);

        return doctor;
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotAuthenticated()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null!);
        var request = new CreateAppointmentRequest(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Unauthorized: User not found!");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPatientNotFound()
    {
        // Arrange
        var identityId = Guid.NewGuid().ToString();
        _httpContextAccessorMock.SetupUserIdentity(identityId);

        var request = new CreateAppointmentRequest(Guid.NewGuid(), Guid.NewGuid());

        _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Patient, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Patient not found or not avaible!");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenScheduleNotFound()
    {
        // Arrange
        var patient = CreateValidPatient();
        _httpContextAccessorMock.SetupUserIdentity(patient.IdentityId.ToString());

        var request = new CreateAppointmentRequest(patient.Uuid, Guid.NewGuid());

        _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Patient, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _repositoriesMock.Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Schedule, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((Schedule)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "Schedule not found!");
    }

    [Fact]
    public async Task Handle_ShouldCreateAppointment_WhenPatientAndScheduleExist()
    {
        // Arrange
        var patient = CreateValidPatient();
        var schedule = CreateValidSchedule();
        var doctor = CreateValidDoctor(schedule.DoctorId);

        _httpContextAccessorMock.SetupUserIdentity(patient.IdentityId!.Value.ToString());

        var request = new CreateAppointmentRequest(patient.Uuid, schedule.Uuid);

        _repositoriesMock.Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Patient, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _repositoriesMock.Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Schedule, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(schedule);

        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Doctor, bool>>>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        _repositoriesMock.Setup(repo => repo.AppointmentRepository.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoriesMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _rabbitMqPublisherMock.Setup(rabbit => rabbit.EnviarMensagem(
            doctor.Name, doctor.Email, patient.Name, It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        
        Assert.NotNull(_httpContextAccessorMock.Object.HttpContext);
        Assert.NotNull(_httpContextAccessorMock.Object.HttpContext!.User);
        Assert.True(_httpContextAccessorMock.Object.HttpContext!.User.Identity!.IsAuthenticated);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
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
