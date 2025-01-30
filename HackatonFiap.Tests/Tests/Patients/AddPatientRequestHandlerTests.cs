using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Api.Controllers.v1;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackatonFiap.Tests.Tests.Patients
{
    public class AddPatientRequestHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositories> _repositoriesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientsController _controller;

        public AddPatientRequestHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositoriesMock = new Mock<IRepositories>();
            _mapperMock = new Mock<IMapper>();

            var httpContext = new DefaultHttpContext();
            _controller = new PatientsController(_mediatorMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task AddPatientAsync_ShouldReturnOk_WhenRequestIsSuccessful()
        {
            // Arrange
            var request = new AddPatientRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                RG = "RG123456"
            };
            
            var patient = new Patient(request.RG)
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                CPF = request.CPF
            };

            var response = new AddPatientResponse
            {
                Id = 1,
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                CPF = request.CPF,
                RG = request.RG
            };
            
            _mapperMock
                .Setup(m => m.Map<Patient>(It.IsAny<AddPatientRequest>()))
                .Returns(patient);

            _repositoriesMock
                .Setup(r => r.PatientRepository.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _repositoriesMock
                .Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<AddPatientResponse>(It.IsAny<Patient>()))
                .Returns(response);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(response));

            // Act
            var result = await _controller.AddPatientAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<AddPatientResponse>(objectResult.Value);
        }


        [Fact]
        public async Task AddPatientAsync_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new AddPatientRequest
            {
                Name = "",
                LastName = "Doe",
                Email = "invalid_email",
                CPF = "",
                RG = ""
            };

            var error = new Error("400")
                .WithMetadata("Name", new[] { "Name is required." })
                .WithMetadata("CPF", new[] { "CPF is required." })
                .WithMetadata("RG", new[] { "RG is required." });

            var failedResult = Result.Fail<AddPatientResponse>(error);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failedResult);

            // Act
            var result = await _controller.AddPatientAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal("Bad Request", problemDetails.Title);
            Assert.Contains("Name", problemDetails.Errors);
            Assert.Contains("CPF", problemDetails.Errors);
            Assert.Contains("RG", problemDetails.Errors);
        }

        [Fact]
        public async Task AddPatientAsync_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new AddPatientRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                RG = "RG123456"
            };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.AddPatientAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal("Internal Server Error", problemDetails.Title);
            Assert.Equal("Unexpected error", problemDetails.Detail);
        }
    }
}
