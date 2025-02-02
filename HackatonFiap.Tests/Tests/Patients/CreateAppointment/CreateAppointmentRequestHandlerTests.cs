using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.CreateAppointment;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using Moq;

namespace HackatonFiap.Tests.Tests.Patients.CreateAppointment
{
    public class CreateAppointmentRequestHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly CreateAppointmentRequestHandler _handler;

        public CreateAppointmentRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            Mock<IMapper> mapperMock = new();
            _handler = new CreateAppointmentRequestHandler(_repositoriesMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateAppointment_WhenPatientAndScheduleExist()
        {
            // Arrange
            var patientUuid = Guid.NewGuid();
            var scheduleUuid = Guid.NewGuid();
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            var schedule = new Schedule(new DateTime(2024, 6, 1, 8, 0, 0), 30, new Doctor(Guid.NewGuid(), "Dr. Smith", "Doe", "dr.smith@example.com", "09876543211", "CRM654321"));

            var request = new CreateAppointmentRequest(patientUuid, scheduleUuid);

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(schedule);

            _repositoriesMock
                .Setup(repo => repo.AppointmentRepository.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _repositoriesMock
                .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            _repositoriesMock.Verify(repo => repo.AppointmentRepository.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPatientNotFoundOrDeleted()
        {
            // Arrange
            var request = new CreateAppointmentRequest(Guid.NewGuid(), Guid.NewGuid());


            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patient)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Patient not found or not avaible!", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenScheduleNotFound()
        {
            // Arrange
            var patientUuid = Guid.NewGuid();
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            
            var request = new CreateAppointmentRequest(patientUuid, Guid.NewGuid());

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Schedule)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Schedule not found!", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenScheduleNotAvailable()
        {
            // Arrange
            var patientUuid = Guid.NewGuid();
            var scheduleUuid = Guid.NewGuid();
            var patient = new Patient(patientUuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            var schedule = new Schedule(new DateTime(2024, 6, 1, 8, 0, 0), 30, new Doctor(Guid.NewGuid(), "Dr. Smith", "Doe", "dr.smith@example.com", "09876543211", "CRM654321"));
            schedule.SetAppointment();

            var request = new CreateAppointmentRequest(patientUuid, scheduleUuid);

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(schedule);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Schedule not avaliable!", result.Errors.Select(e => e.Message));
        }
        
        [Fact]
        public async Task Handle_ShouldAllowFirstRequest_AndRejectSecond_WhenBothTryBookingSameSchedule()
        {
            // Arrange
            var patient1Uuid = Guid.NewGuid();
            var patient2Uuid = Guid.NewGuid();
            var scheduleUuid = Guid.NewGuid();

            var doctor = new Doctor(Guid.NewGuid(), "Dr. Smith", "Doe", "dr.smith@example.com", "09876543211", "CRM654321");
            var patient1 = new Patient(patient1Uuid, "John", "Doe", "john.doe@example.com", "12345678900", "RG123456");
            var patient2 = new Patient(patient2Uuid, "Jane", "Doe", "jane.doe@example.com", "09876543211", "RG654321");
            var schedule = new Schedule(new DateTime(2024, 6, 1, 8, 0, 0), 30, doctor);

            var request1 = new CreateAppointmentRequest(patient1Uuid, scheduleUuid);
            var request2 = new CreateAppointmentRequest(patient2Uuid, scheduleUuid);

            _repositoriesMock
                .Setup(repo => repo.PatientRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<string?>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<Patient, bool>> predicate, string? _, bool _, CancellationToken _) =>
                {
                    return predicate.Compile().Invoke(patient1) ? patient1 : patient2;
                });


            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(schedule);

            _repositoriesMock
                .Setup(repo => repo.AppointmentRepository.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _repositoriesMock
                .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            async Task<Result> TryBookAppointment(CreateAppointmentRequest request)
            {
                return await _handler.Handle(request, CancellationToken.None);
            }

            // Act
            var results = await Task.WhenAll(TryBookAppointment(request1), TryBookAppointment(request2));
            var result1 = results[0];
            var result2 = results[1];

            // Assert
            Assert.True(result1.IsSuccess);
            Assert.True(result2.IsFailed);
            Assert.Contains("Schedule not avaliable!", result2.Errors.Select(e => e.Message));
        }
    }
}
