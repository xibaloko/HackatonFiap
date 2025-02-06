using HackatonFiap.Identity.Application.UseCases.RenewAccess;
using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Domain.Services;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HackatonFiap.Tests.Tests.Identity.RenewAccess
{
    public class RenewAccessRequestHandlerTests
    {
        private readonly Mock<IAuthenticationTokenService> _authenticationTokenServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly RenewAccessRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public RenewAccessRequestHandlerTests()
        {
            _authenticationTokenServiceMock = new Mock<IAuthenticationTokenService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _handler = new RenewAccessRequestHandler(
                _authenticationTokenServiceMock.Object, 
                _userManagerMock.Object);

            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldRenewAccess_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "testuser", Email = "test@example.com" };
            var request = new RenewAccessRequest { Email = user.Email, RefreshToken = "valid_refresh_token" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _authenticationTokenServiceMock
                .Setup(m => m.RenewAccessToken(request.RefreshToken))
                .Returns((true, "new_access_token"));

            _authenticationTokenServiceMock
                .Setup(m => m.GetAccessTokenExpiration())
                .Returns(DateTime.UtcNow.AddMinutes(30));

            _authenticationTokenServiceMock
                .Setup(m => m.GenerateRefreshToken(user.Id, It.IsAny<IEnumerable<string>>()))
                .Returns("new_refresh_token");

            _authenticationTokenServiceMock
                .Setup(m => m.GetRefreshTokenExpiration())
                .Returns(DateTime.UtcNow.AddDays(7));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Id, result.Value.IdentityId.ToString());
            Assert.Equal("new_access_token", result.Value.AccessToken);
            Assert.Equal("new_refresh_token", result.Value.RefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenEmailNotFound()
        {
            // Arrange
            var request = new RenewAccessRequest { Email = "nonexistent@example.com", RefreshToken = "valid_refresh_token" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Invalid credentials.", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "testuser", Email = "test@example.com" };
            var request = new RenewAccessRequest { Email = user.Email, RefreshToken = "invalid_refresh_token" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _authenticationTokenServiceMock
                .Setup(m => m.RenewAccessToken(request.RefreshToken))
                .Returns((false, null));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Invalid token.", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new RenewAccessRequest { Email = "test@example.com", RefreshToken = "valid_refresh_token" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
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
