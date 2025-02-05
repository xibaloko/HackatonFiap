using FluentResults;
using HackatonFiap.Identity.Application.Configurations.FluentResults;
using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HackatonFiap.Identity.Application.UseCases.Login;

public sealed class LoginRequestHandler : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    private readonly IAuthenticationTokenService _authenticationTokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private const string invalidCredentialsMessage = "Invalid credentials.";

    public LoginRequestHandler(
        IAuthenticationTokenService authenticationTokenService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
        )
    {
        _authenticationTokenService = authenticationTokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = request.Username.Contains("@")
            ? await _userManager.FindByEmailAsync(request.Username).WaitAsync(cancellationToken)
            : await _userManager.FindByNameAsync(request.Username).WaitAsync(cancellationToken);

        if (user is null)
            return Result.Fail(ErrorHandler.HandleBadRequest(invalidCredentialsMessage));

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: false).WaitAsync(cancellationToken);

        if (!result.Succeeded)
            return Result.Fail(ErrorHandler.HandleBadRequest(invalidCredentialsMessage));

        var roles = await _userManager.GetRolesAsync(user);

        LoginResponse response = GenerateResponse(user.Id, roles);

        return Result.Ok(response);
    }

    private LoginResponse GenerateResponse(string identityId, IEnumerable<string> roles)
    {
        string accessToken = _authenticationTokenService.GenerateAccessToken(identityId, roles);
        int accessTokenExpiratesIn = (int)(_authenticationTokenService.GetAccessTokenExpiration() - DateTime.UtcNow).TotalSeconds;
        string refreshToken = _authenticationTokenService.GenerateRefreshToken(identityId, roles);
        int refreshTokenExpiratesIn = (int)(_authenticationTokenService.GetRefreshTokenExpiration() - DateTime.UtcNow).TotalSeconds;

        return new LoginResponse(Guid.Parse(identityId), accessToken, accessTokenExpiratesIn, refreshToken, refreshTokenExpiratesIn);
    }
}
