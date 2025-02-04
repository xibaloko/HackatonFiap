using HackatonFiap.Identity.Application.UseCases.Login;
using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Domain.Services;
using HackatonFiap.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HackatonFiap.Tests.Tests.Identity.Login
{
    public class LoginRequestHandlerTests
    {
        private readonly Mock<IAuthenticationTokenService> _authenticationTokenServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly LoginRequestHandler _handler;
        private readonly ExeptionHandling _exeptionHandling;

        public LoginRequestHandlerTests()
        {
            _authenticationTokenServiceMock = new Mock<IAuthenticationTokenService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object, 
                Mock.Of<IHttpContextAccessor>(), 
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), 
                null, null, null, null);

            _handler = new LoginRequestHandler(
                _authenticationTokenServiceMock.Object, 
                _userManagerMock.Object, 
                _signInManagerMock.Object);

            _exeptionHandling = new ExeptionHandling();
        }

        [Fact]
        public async Task Handle_ShouldAuthenticate_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "testuser", Email = "test@example.com" };
            var request = new LoginRequest { Email = user.Email, Password = "ValidPassword123!" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(m => m.PasswordSignInAsync(user.UserName!, request.Password, false, false))
                .ReturnsAsync(SignInResult.Success);

            //_authenticationTokenServiceMock
            //    .Setup(m => m.GenerateAccessToken(user.UserName!))
            //    .Returns("valid_access_token");

            _authenticationTokenServiceMock
                .Setup(m => m.GetAccessTokenExpiration())
                .Returns(DateTime.UtcNow.AddMinutes(30));

            //_authenticationTokenServiceMock
            //    .Setup(m => m.GenerateRefreshToken(user.UserName!))
            //    .Returns("valid_refresh_token");

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
            Assert.Equal("valid_access_token", result.Value.AccessToken);
            Assert.Equal("valid_refresh_token", result.Value.RefreshToken);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenEmailNotFound()
        {
            // Arrange
            var request = new LoginRequest { Email = "nonexistent@example.com", Password = "InvalidPassword!" };

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
        public async Task Handle_ShouldReturnError_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "testuser", Email = "test@example.com" };
            var request = new LoginRequest { Email = user.Email, Password = "WrongPassword!" };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _signInManagerMock
                .Setup(m => m.PasswordSignInAsync(user.UserName!, request.Password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Contains("Invalid credentials.", result.Errors.Select(e => e.Message));
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@example.com", Password = "ValidPassword123!" };

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
