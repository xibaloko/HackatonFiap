using AutoMapper;
using HackatonFiap.Identity.Application.UseCases.CreateAccount;
using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HackatonFiap.Tests.Tests.Identity.CreateAccount
{
    public class CreateAccountRequestHandlerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateAccountRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public CreateAccountRequestHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mapperMock = new Mock<IMapper>();
            _handler = new CreateAccountRequestHandler(_userManagerMock.Object, _mapperMock.Object);
            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldCreateAccount_WhenDataIsValid()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "StrongPassword123!",
                Role = "User"
            };

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };

            _mapperMock
                .Setup(m => m.Map<ApplicationUser>(request))
                .Returns(user);

            _userManagerMock
                .Setup(m => m.CreateAsync(user, request.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(m => m.AddToRoleAsync(user, request.Role))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _userManagerMock.Verify(m => m.CreateAsync(user, request.Password), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(user, request.Role), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenUserCreationFails()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "WeakPassword",
                Role = "User"
            };

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };

            _mapperMock
                .Setup(m => m.Map<ApplicationUser>(request))
                .Returns(user);

            _userManagerMock
                .Setup(m => m.CreateAsync(user, request.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Weak password." }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("It was not possible to create an account.", result.Errors.Select(e => e.Message));
            _userManagerMock.Verify(m => m.CreateAsync(user, request.Password), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new CreateAccountRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "StrongPassword123!",
                Role = "User"
            };

            _mapperMock
                .Setup(m => m.Map<ApplicationUser>(request))
                .Throws(new Exception("Unexpected error"));

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
