using System.Linq.Expressions;
using AutoMapper;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using Moq;

namespace HackatonFiap.Tests.Tests.Doctors.GetDoctorByUuid
{
    public class GetDoctorByUuidRequestHandlerTests
    {
        //    private readonly Mock<IRepositories> _repositoriesMock;
        //    private readonly Mock<IMapper> _mapperMock;
        //    private readonly GetDoctorByUuidRequestHandler _handler;

        //    public GetDoctorByUuidRequestHandlerTests()
        //    {
        //        _repositoriesMock = new Mock<IRepositories>();
        //        _mapperMock = new Mock<IMapper>();
        //        _handler = new GetDoctorByUuidRequestHandler(_repositoriesMock.Object, _mapperMock.Object);
        //    }

        //    [Fact]
        //    public async Task Handle_ShouldReturnDoctor_WhenDoctorExists()
        //    {
        //        // Arrange
        //        var uuid = Guid.NewGuid();
        //        var doctor = new Doctor("CRM123456")
        //        {
        //            Name = "Dr. John",
        //            LastName = "Doe",
        //            Email = "john.doe@example.com",
        //            CPF = "12345678900"
        //        };

        //        var propertyInfo = typeof(Doctor).BaseType?.GetProperty("Uuid", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //        propertyInfo?.SetValue(doctor, uuid);

        //        var response = new GetDoctorByUuidResponse
        //        {
        //            Uuid = uuid,
        //            Name = "Dr. John",
        //            LastName = "Doe",
        //            Email = "john.doe@example.com",
        //            CPF = "12345678900",
        //            CRM = "CRM123456"
        //        };

        //        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
        //                It.IsAny<Expression<Func<Doctor, bool>>>(),
        //                It.IsAny<string?>(),
        //                It.IsAny<bool>(),
        //                It.IsAny<CancellationToken>()))
        //            .ReturnsAsync(doctor);

        //        _mapperMock.Setup(mapper => mapper.Map<GetDoctorByUuidResponse>(doctor))
        //            .Returns(response);

        //        // Act
        //        var request = new GetDoctorByUuidRequest(uuid);
        //        var result = await _handler.Handle(request, CancellationToken.None);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.True(result.IsSuccess);
        //        Assert.Equal("Dr. John", result.Value.Name);
        //        Assert.Equal(uuid, result.Value.Uuid);
        //    }

        //    [Fact]
        //    public async Task Handle_ShouldReturnError_WhenDoctorNotFound()
        //    {
        //        // Arrange
        //        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
        //                It.IsAny<Expression<Func<Doctor, bool>>>(),
        //                It.IsAny<string?>(),
        //                It.IsAny<bool>(),
        //                It.IsAny<CancellationToken>()))
        //            .ReturnsAsync((Doctor)null!);

        //        // Act
        //        var request = new GetDoctorByUuidRequest(Guid.NewGuid());
        //        var result = await _handler.Handle(request, CancellationToken.None);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.False(result.IsSuccess);
        //        Assert.Contains("Doctor not found.", result.Errors.Select(e => e.Message));
        //    }

        //    [Fact]
        //    public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        //    {
        //        // Arrange
        //        _repositoriesMock.Setup(repo => repo.DoctorRepository.FirstOrDefaultAsync(
        //                It.IsAny<Expression<Func<Doctor, bool>>>(),
        //                It.IsAny<string?>(),
        //                It.IsAny<bool>(),
        //                It.IsAny<CancellationToken>()))
        //            .ThrowsAsync(new Exception("Database error"));

        //        // Act & Assert
        //        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        //            await _handler.Handle(new GetDoctorByUuidRequest(Guid.NewGuid()), CancellationToken.None));

        //        Assert.Equal("Database error", exception.Message);
        //    }
    }
}
