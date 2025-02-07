using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Schedules.AddSchedule
{
    public class AddScheduleRequestHandlerTests
    {
        private readonly Mock<IUnitOfWork> _repositoriesMock;
        //private readonly AddScheduleRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public AddScheduleRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IUnitOfWork>();
            //_handler = new AddScheduleRequestHandler(_repositoriesMock.Object);
            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldAddSchedules_WhenDoctorExistsAndNoConflict()
        {
            //// Arrange
            //var doctorUuid = Guid.NewGuid();
            //var doctor = new Doctor(doctorUuid, "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            //var request = new AddScheduleRequest
            //{
            //    DoctorUuid = doctorUuid,
            //    Date = new DateOnly(2024, 6, 1),
            //    InitialHour = new TimeOnly(8, 0),
            //    FinalHour = new TimeOnly(10, 0),
            //    Duration = 30,
            //    Avaliable = true
            //};

            //_repositoriesMock
            //    .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            //        It.IsAny<Expression<Func<Doctor, bool>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(doctor);

            //_repositoriesMock
            //    .Setup(repo => repo.ScheduleRepository.GetAllAsync(
            //        It.IsAny<Expression<Func<Schedule, bool>>>(),
            //        It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new List<Schedule>());


            //_repositoriesMock
            //    .Setup(repo => repo.ScheduleRepository.AddBulkAsync(
            //        It.IsAny<IEnumerable<Schedule>>(),
            //        It.IsAny<CancellationToken>()))
            //    .Returns(Task.CompletedTask);


            //_repositoriesMock
            //    .Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>()))
            //    .Returns(Task.CompletedTask);

            //// Act
            //var result = await _handler.Handle(request, CancellationToken.None);

            //// Assert
            //Assert.NotNull(result);
            //Assert.True(result.IsSuccess);
            //_repositoriesMock.Verify(repo => repo.ScheduleRepository.AddBulkAsync(
            //        It.IsAny<IEnumerable<Schedule>>(),
            //        It.IsAny<CancellationToken>()),
            //    Times.Once);
            //_repositoriesMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
        {
            //// Arrange
            //var request = new AddScheduleRequest
            //{
            //    DoctorUuid = Guid.NewGuid(),
            //    Date = new DateOnly(2024, 6, 1),
            //    InitialHour = new TimeOnly(8, 0),
            //    FinalHour = new TimeOnly(10, 0),
            //    Duration = 30,
            //    Avaliable = true
            //};

            //_repositoriesMock
            //    .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            //        It.IsAny<Expression<Func<Doctor, bool>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync((Doctor)null!);

            //// Act
            //var result = await _handler.Handle(request, CancellationToken.None);

            //// Assert
            //Assert.NotNull(result);
            //Assert.True(result.IsFailed);
            //Assert.Contains("Doctor not found!", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenSchedulesConflict()
        {
            // Arrange
            //var doctorUuid = Guid.NewGuid();
            //var doctor = new Doctor(doctorUuid, "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            //var request = new AddScheduleRequest
            //{
            //    DoctorUuid = doctorUuid,
            //    Date = new DateOnly(2024, 6, 1),
            //    InitialHour = new TimeOnly(8, 0),
            //    FinalHour = new TimeOnly(10, 0),
            //    Duration = 30,
            //    Avaliable = true
            //};

            //var conflictingSchedules = new List<Schedule>
            //{
            //    new Schedule(new DateTime(2024, 6, 1, 8, 30, 0), 30, doctor)
            //};

            //_repositoriesMock
            //    .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            //        It.IsAny<Expression<Func<Doctor, bool>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(doctor);

            //_repositoriesMock
            //    .Setup(repo => repo.ScheduleRepository.GetAllAsync(
            //        It.IsAny<Expression<Func<Schedule, bool>>>(),
            //        It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(conflictingSchedules);


            //// Act
            //var result = await _handler.Handle(request, CancellationToken.None);

            //// Assert
            //Assert.NotNull(result);
            //Assert.True(result.IsFailed);
            //Assert.Contains("Exists Schedules conflicted", result.Errors.Select(e => e.Message).First());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            //var request = new AddScheduleRequest
            //{
            //    DoctorUuid = Guid.NewGuid(),
            //    Date = new DateOnly(2024, 6, 1),
            //    InitialHour = new TimeOnly(8, 0),
            //    FinalHour = new TimeOnly(10, 0),
            //    Duration = 30,
            //    Avaliable = true
            //};

            //_repositoriesMock
            //    .Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
            //        It.IsAny<Expression<Func<Doctor, bool>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            //var result = await _exeptionHandling.ExecuteWithExceptionHandling(() =>
            //    _handler.Handle(request, CancellationToken.None));

            // Assert
            //Assert.NotNull(result);
            //Assert.True(result.IsFailed);
            //Assert.Contains("Unexpected error", result.Errors.Select(e => e.Message));
        }
    }
}
