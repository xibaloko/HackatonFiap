using FluentResults;
using HackatonFiap.HealthScheduling.Api.Controllers.v1;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;
using HackatonFiap.Tests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackatonFiap.Tests.Tests.Doctors.AddDoctor
{
    public class AddDoctorRequestHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DoctorsController _controller;

        public AddDoctorRequestHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            
            var httpContext = new DefaultHttpContext();
            _controller = new DoctorsController(_mediatorMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task AddDoctorAsync_ShouldReturnOk_WhenRequestIsSuccessful()
        {
            // Arrange
            var specialtyUuid = Guid.NewGuid();

            var request = new AddDoctorRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                CRM = "123456-SP",
                Password = "SecurePassword@123",
                Role = "Doctor",
                MedicalSpecialtyUuid = specialtyUuid
            };

            var response = new AddDoctorResponse
            {
                Id = 1,
                Uuid = Guid.NewGuid(),
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                CPF = request.CPF,
                CRM = request.CRM
            };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(response));

            // Act
            var result = await _controller.AddDoctorAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<AddDoctorResponse>(objectResult.Value);
        }

        [Fact]
        public async Task AddDoctorAsync_ShouldReturnBadRequest_WhenMedicalSpecialtyNotFound()
        {
            var exceptionHandling = new ExeptionHandling();

            // Arrange
            var request = new AddDoctorRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                CRM = "123456-SP",
                Password = "SecurePassword@123",
                Role = "Doctor",
                MedicalSpecialtyUuid = Guid.NewGuid()
            };

            var errorMessage = "Medical Specialty not found!";
            var error = new Error("400").WithMetadata("Message", new[] { errorMessage });

            var failedResult = Result.Fail<AddDoctorResponse>(error);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failedResult);

            // Act
            var result = await exceptionHandling.ExecuteWithBadRequestHandling(() => _controller.AddDoctorAsync(request, CancellationToken.None));

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal("Bad Request", problemDetails.Title);
    
            Assert.Contains("Message", problemDetails.Errors);
            Assert.Contains(errorMessage, problemDetails.Errors["Message"]);
        }


        

        [Fact]
        public async Task AddDoctorAsync_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new AddDoctorRequest
            {
                Name = "",
                LastName = "Doe",
                Email = "invalid_email",
                CPF = "",
                CRM = "123456-SP",
                Password = "",
                Role = "",
                MedicalSpecialtyUuid = Guid.NewGuid()
            };

            var error = new Error("400")
                .WithMetadata("Name", new[] { "Name is required." })
                .WithMetadata("CPF", new[] { "CPF is required." })
                .WithMetadata("Password", new[] { "Password is required." })
                .WithMetadata("Role", new[] { "Role is required." });

            var failedResult = Result.Fail<AddDoctorResponse>(error);

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failedResult);

            // Act
            var result = await _controller.AddDoctorAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal("Bad Request", problemDetails.Title);
            Assert.Contains("Name", problemDetails.Errors);
            Assert.Contains("CPF", problemDetails.Errors);
            Assert.Contains("Password", problemDetails.Errors);
            Assert.Contains("Role", problemDetails.Errors);
        }

        [Fact]
        public async Task AddDoctorAsync_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            var exeptionHandling = new ExeptionHandling();

            // Arrange
            var request = new AddDoctorRequest
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CPF = "12345678900",
                CRM = "123456-SP",
                Password = "SecurePassword@123",
                Role = "Doctor",
                MedicalSpecialtyUuid = Guid.NewGuid()
            };

            _mediatorMock
                .Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await exeptionHandling.ExecuteWithExceptionHandling(() => _controller.AddDoctorAsync(request, CancellationToken.None));

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal("Internal Server Error", problemDetails.Title);
            Assert.Equal("Unexpected error", problemDetails.Detail);
        }
    }
}
