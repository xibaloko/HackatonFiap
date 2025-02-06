using System.Linq.Expressions;
using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;

namespace HackatonFiap.Tests.Tests.Doctors.GetAllDoctors
{
    public class GetAllDoctorsRequestHandlerTests
    {
        private readonly Mock<IUnitOfWork> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllDoctorsRequestHandler _handler;

        public GetAllDoctorsRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllDoctorsRequestHandler(_repositoriesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnDoctors_WhenDoctorsExist()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "CRM123456"),
                new Doctor(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", "09876543211", "CRM654321")
            };

            var response = new GetAllDoctorsResponse
            {
                Doctors = new List<DoctorResponse>
                {
                    new DoctorResponse
                    {
                        Uuid = doctors[0].IdentityId!.Value,
                        Name = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        CPF = "12345678900",
                        CRM = "CRM123456"
                    },
                    new DoctorResponse
                    {
                        Uuid = doctors[1].IdentityId!.Value,
                        Name = "Jane",
                        LastName = "Doe",
                        Email = "jane.doe@example.com",
                        CPF = "09876543211",
                        CRM = "CRM654321"
                    }
                }
            };

            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
                    It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(doctors);

            _mapperMock
                .Setup(mapper => mapper.Map<GetAllDoctorsResponse>(doctors))
                .Returns(response);

            // Act
            var result = await _handler.Handle(new GetAllDoctorsRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Doctors.Count());

            var doctorsList = result.Value.Doctors.ToList();
            Assert.Equal("John", doctorsList[0].Name);
            Assert.Equal("Jane", doctorsList[1].Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoDoctorsExist()
        {
            // Arrange
            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
                    It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Doctor>());

            _mapperMock
                .Setup(mapper => mapper.Map<GetAllDoctorsResponse>(It.IsAny<List<Doctor>>()))
                .Returns(new GetAllDoctorsResponse { Doctors = Enumerable.Empty<DoctorResponse>() });

            // Act
            var result = await _handler.Handle(new GetAllDoctorsRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Doctors);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            var exeptionHandling = new ExeptionHandling();
            
            // Arrange
            _repositoriesMock
                .Setup(repo => repo.DoctorRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Doctor, bool>>>(),
                    It.IsAny<Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await exeptionHandling.ExecuteWithExceptionHandling(() => _handler.Handle(new GetAllDoctorsRequest(), CancellationToken.None));


            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Database error", result.Errors.Select(e => e.Message));
        }
    }
}
