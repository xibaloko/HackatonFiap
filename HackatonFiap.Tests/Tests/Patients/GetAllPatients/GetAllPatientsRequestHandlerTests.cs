using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HackatonFiap.Tests.Tests.Patients.GetAllPatients
{
    public class GetAllPatientsRequestHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUnitOfWork> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPatientsRequestHandler _handler;

        public GetAllPatientsRequestHandlerTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _repositoriesMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            // Simulando um usuário autenticado no contexto HTTP
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            _handler = new GetAllPatientsRequestHandler(_httpContextAccessorMock.Object, _repositoriesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPatients_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "12345678900", "RG123456"),
                new Patient(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", "09876543211", "RG654321")
            };

            var response = new GetAllPatientsResponse
            {
                Patients = new List<PatientResponse>
                {
                    new PatientResponse
                    {
                        Uuid = patients[0].IdentityId!.Value,
                        Name = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        CPF = "12345678900",
                        RG = "RG123456"
                    },
                    new PatientResponse
                    {
                        Uuid = patients[1].IdentityId!.Value,
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
        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            var exceptionHandling = new ExeptionHandling();
                
            // Arrange
            _repositoriesMock
                .Setup(repo => repo.PatientRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Patient, bool>>>(),
                    It.IsAny<Func<IQueryable<Patient>, IOrderedQueryable<Patient>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act
            var result = await exceptionHandling.ExecuteWithExceptionHandling(() => _handler.Handle(new GetAllPatientsRequest(), CancellationToken.None));

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Erro ao acessar o banco de dados", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null!);

            var handler = new GetAllPatientsRequestHandler(_httpContextAccessorMock.Object, _repositoriesMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(new GetAllPatientsRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Unauthorized: User not found!", result.Errors.Select(e => e.Message));
        }
    }
}
