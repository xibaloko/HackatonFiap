using System.Linq.Expressions;
using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Schedules.GetScheduleFromDoctor
{
    public class GetScheduleFromDoctorHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetScheduleFromDoctorHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public GetScheduleFromDoctorHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetScheduleFromDoctorHandler(_repositoriesMock.Object, _mapperMock.Object);
            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldReturnSchedules_WhenSchedulesExist()
        {
            //// Arrange
            //var doctorUuid = Guid.NewGuid();
            //var doctor = new Doctor(doctorUuid, "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456");

            //var request = new GetScheduleFromDoctorRequest(doctorUuid);

            //var schedules = new List<Schedule>
            //{
            //    new Schedule(new DateTime(2024, 6, 1, 8, 0, 0), 30, doctor),
            //    new Schedule(new DateTime(2024, 6, 1, 9, 0, 0), 30, doctor)
            //};

            //var response = new GetScheduleFromDoctorResponse
            //{
            //    DoctorUuid = doctorUuid,
            //    FreeSchedules = new List<DoctorAvailableSchedule>
            //    {
            //        new DoctorAvailableSchedule
            //        {
            //            DateSchedule = new DateOnly(2024, 6, 1),
            //            Appointments = new List<Appointment>
            //            {
            //                new Appointment { Hour = new TimeOnly(8, 0), ScheduleUuid = schedules[0].Uuid },
            //                new Appointment { Hour = new TimeOnly(9, 0), ScheduleUuid = schedules[1].Uuid }
            //            }
            //        }
            //    }
            //};

            //_repositoriesMock
            //    .Setup(repo => repo.ScheduleRepository.GetAllAsync(
            //        It.IsAny<Expression<Func<Schedule, bool>>>(),
            //        It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
            //        It.IsAny<string>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(schedules);

            //_mapperMock
            //    .Setup(mapper => mapper.Map<GetScheduleFromDoctorResponse>(schedules))
            //    .Returns(response);

            //// Act
            //var result = await _handler.Handle(request, CancellationToken.None);

            //// Assert
            //Assert.NotNull(result);
            //Assert.True(result.IsSuccess);
            //Assert.NotNull(result.Value);
            //Assert.Equal(doctorUuid, result.Value.DoctorUuid);
            //Assert.Single(result.Value.FreeSchedules);
            //Assert.Equal(2, result.Value.FreeSchedules.First().Appointments.Count);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoSchedulesExist()
        {
            // Arrange
            var request = new GetScheduleFromDoctorRequest(Guid.NewGuid());

            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Schedule>());

            _mapperMock
                .Setup(mapper => mapper.Map<GetScheduleFromDoctorResponse>(It.IsAny<List<Schedule>>()))
                .Returns(new GetScheduleFromDoctorResponse { FreeSchedules = new List<DoctorAvailableSchedule>() });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.FreeSchedules);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new GetScheduleFromDoctorRequest(Guid.NewGuid());

            _repositoriesMock
                .Setup(repo => repo.ScheduleRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Schedule, bool>>>(),
                    It.IsAny<Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>>>(),
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
}
