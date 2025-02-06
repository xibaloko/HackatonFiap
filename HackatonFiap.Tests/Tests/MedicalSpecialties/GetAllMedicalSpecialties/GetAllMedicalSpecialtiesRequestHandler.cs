using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.Tests.Helpers;
using Moq;
using Xunit;

namespace HackatonFiap.Tests.Tests.MedicalSpecialties.GetAllMedicalSpecialties;

public class GetAllMedicalSpecialtiesRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllMedicalSpecialtiesRequestHandler _handler;
    private readonly ExeptionHandling _exeptionHandling;

    public GetAllMedicalSpecialtiesRequestHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllMedicalSpecialtiesRequestHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        _exeptionHandling = new ExeptionHandling();
    }

    [Fact]
    public async Task Handle_ShouldReturnMedicalSpecialties_WhenMedicalSpecialtiesExist()
    {
        // Arrange
        var specialties = new List<MedicalSpecialty>
        {
            CreateMedicalSpecialtyWithUuid("Cardiology"),
            CreateMedicalSpecialtyWithUuid("Dermatology")
        };


        var response = new GetAllMedicalSpecialtiesResponse
        {
            MedicalSpecialties = specialties.Select(s => new MedicalSpecialtyResponse
            {
                Uuid = s.Uuid,
                Description = s.Description
            })
        };

        _unitOfWorkMock
            .Setup(repo => repo.MedicalSpecialtyRepository.GetAllAsync(
                It.IsAny<Expression<Func<MedicalSpecialty, bool>>>(),
                It.IsAny<Func<IQueryable<MedicalSpecialty>, IOrderedQueryable<MedicalSpecialty>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(specialties);

        _mapperMock
            .Setup(mapper => mapper.Map<GetAllMedicalSpecialtiesResponse>(specialties))
            .Returns(response);

        // Act
        var result = await _handler.Handle(new GetAllMedicalSpecialtiesRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.MedicalSpecialties.Count());
    }
    
    private MedicalSpecialty CreateMedicalSpecialtyWithUuid(string description)
    {
        var specialty = new MedicalSpecialty(description);

        var uuidProperty = typeof(EntityBase).GetProperty("Uuid", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (uuidProperty == null)
            throw new InvalidOperationException("Property 'Uuid' not found.");

        uuidProperty.SetValue(specialty, Guid.NewGuid());

        return specialty;
    }




    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoMedicalSpecialtiesExist()
    {
        // Arrange
        _unitOfWorkMock
            .Setup(repo => repo.MedicalSpecialtyRepository.GetAllAsync(
                It.IsAny<Expression<Func<MedicalSpecialty, bool>>>(),
                It.IsAny<Func<IQueryable<MedicalSpecialty>, IOrderedQueryable<MedicalSpecialty>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new List<MedicalSpecialty>());

        _mapperMock
            .Setup(mapper => mapper.Map<GetAllMedicalSpecialtiesResponse>(It.IsAny<List<MedicalSpecialty>>()!))
            .Returns(new GetAllMedicalSpecialtiesResponse { MedicalSpecialties = Enumerable.Empty<MedicalSpecialtyResponse>() });

        // Act
        var result = await _handler.Handle(new GetAllMedicalSpecialtiesRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.MedicalSpecialties);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        _unitOfWorkMock
            .Setup(repo => repo.MedicalSpecialtyRepository.GetAllAsync(
                It.IsAny<Expression<Func<MedicalSpecialty, bool>>>(),
                It.IsAny<Func<IQueryable<MedicalSpecialty>, IOrderedQueryable<MedicalSpecialty>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _exeptionHandling.ExecuteWithExceptionHandling(() =>
            _handler.Handle(new GetAllMedicalSpecialtiesRequest(), CancellationToken.None));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Contains("Unexpected error", result.Errors.Select(e => e.Message));
    }
}
