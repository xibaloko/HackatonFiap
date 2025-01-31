using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using Moq;
using System.Linq.Expressions;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;

namespace HackatonFiap.Tests.Tests.Patients.GetAllPatients
{
    public class GetAllPatientsRequestHandlerTests
    {
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPatientsRequestHandler _handler;

        public GetAllPatientsRequestHandlerTests()
        {
            _repositoriesMock = new Mock<IRepositories>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllPatientsRequestHandler(_repositoriesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPatients_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient("RG123456")
                {
                    Name = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    CPF = "12345678900"
                },
                new Patient("RG654321")
                {
                    Name = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@example.com",
                    CPF = "09876543211"
                }
            };

            var response = new GetAllPatientsResponse
            {
                Patients = new List<PatientResponse> 
                {
                    new PatientResponse
                    {
                        Uuid = Guid.NewGuid(),
                        Name = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        CPF = "12345678900",
                        RG = "RG123456"
                    },
                    new PatientResponse
                    {
                        Uuid = Guid.NewGuid(),
                        Name = "Jane",
                        LastName = "Doe",
                        Email = "jane.doe@example.com",
                        CPF = "09876543211",
                        RG = "RG654321"
                    }
                }
            };


            _repositoriesMock
                .Setup(repo => repo.PatientRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<Func<IQueryable<Patient>, IOrderedQueryable<Patient>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(patients);



            _mapperMock
                .Setup(mapper => mapper.Map<GetAllPatientsResponse>(patients))
                .Returns(response);
            
            // Act
            var result = await _handler.Handle(new GetAllPatientsRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Patients.Count());
            
            var patientsList = result.Value.Patients.ToList();
            Assert.Equal("John", patientsList[0].Name);
            Assert.Equal("Jane", patientsList[1].Name);

        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPatientsExist()
        {
            // Arrange
            _repositoriesMock
                .Setup(repo => repo.PatientRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<Func<IQueryable<Patient>, IOrderedQueryable<Patient>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Patient>());

            _mapperMock
                .Setup(mapper => mapper.Map<GetAllPatientsResponse>(It.IsAny<List<Patient>>()))
                .Returns(new GetAllPatientsResponse { Patients = Enumerable.Empty<PatientResponse>() });

            // Act
            var result = await _handler.Handle(new GetAllPatientsRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Patients);
        }


        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repositoriesMock
                .Setup(repo => repo.PatientRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<Func<IQueryable<Patient>, IOrderedQueryable<Patient>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(new GetAllPatientsRequest(), CancellationToken.None));
        }
    }
}
