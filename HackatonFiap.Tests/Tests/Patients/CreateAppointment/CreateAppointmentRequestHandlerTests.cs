using System.Linq.Expressions;
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
using Moq;
using Xunit;

namespace HackatonFiap.Tests.Tests.Patients.CreateAppointment
{
    public class CreateAppointmentRequestHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly Mock<IRabbitMqPublisher> _rabbitMqPublisherMock;
        private readonly CreateAppointmentRequestHandler _handler;
        private readonly Fixture _fixture;

        public CreateAppointmentRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            _rabbitMqPublisherMock = new Mock<IRabbitMqPublisher>();
            _fixture = new Fixture();

            _handler = new CreateAppointmentRequestHandler(
                _repositoriesMock.Object,
                Mock.Of<AutoMapper.IMapper>(),
                _rabbitMqPublisherMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateAppointment_AndPublishMessage_WhenPatientAndScheduleExist()
        {
            // Arrange
            var patientUuid = Guid.NewGuid();
            var scheduleUuid = Guid.NewGuid();
            var doctor = new Doctor(Guid.NewGuid(), "Dr. Smith", "Doe", "dr.smith@example.com", "09876543211", "CRM654321");
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            var schedule = new Schedule(DateTime.UtcNow, 30, doctor);

            var request = new CreateAppointmentRequest(patientUuid, scheduleUuid);

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

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoriesMock.Verify(repo => repo.AppointmentRepository.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
            _rabbitMqPublisherMock.Verify(rabbit =>
                rabbit.EnviarMensagem(doctor.Name, doctor.Email, patient.Name, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPatientNotFound()
        {
            // Arrange
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
            var patientUuid = Guid.NewGuid();
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");

            var request = new CreateAppointmentRequest(patientUuid, Guid.NewGuid());

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
        public async Task Handle_ShouldReturnError_WhenScheduleNotAvailable()
        {
            // Arrange
            var patientUuid = Guid.NewGuid();
            var scheduleUuid = Guid.NewGuid();
            var doctor = new Doctor(Guid.NewGuid(), "Dr. Smith", "Doe", "dr.smith@example.com", "09876543211", "CRM654321");
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            var schedule = new Schedule(DateTime.UtcNow, 30, doctor);
            schedule.SetAppointment(); // Marca o agendamento como indisponível

            var request = new CreateAppointmentRequest(patientUuid, scheduleUuid);

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

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == "Schedule not avaliable!");
        }
    }
}
