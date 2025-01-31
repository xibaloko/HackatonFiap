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
        var user = await _userManager.FindByEmailAsync(request.Email).WaitAsync(cancellationToken);

        if (user is null)
            return Result.Fail(ErrorHandler.HandleBadRequest(invalidCredentialsMessage));

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: false).WaitAsync(cancellationToken);

        if (!result.Succeeded)
            return Result.Fail(ErrorHandler.HandleBadRequest(invalidCredentialsMessage));

        LoginResponse response = GenerateResponse(user);

        return Result.Ok(response);
    }

    private LoginResponse GenerateResponse(ApplicationUser user)
    {
        Guid identityId = Guid.Parse(user.Id);
        string accessToken = _authenticationTokenService.GenerateAccessToken(user.UserName!);
        int accessTokenExpiratesIn = (int)(_authenticationTokenService.GetAccessTokenExpiration() - DateTime.UtcNow).TotalSeconds;
        string refreshToken = _authenticationTokenService.GenerateRefreshToken(user.UserName!);
        int refreshTokenExpiratesIn = (int)(_authenticationTokenService.GetRefreshTokenExpiration() - DateTime.UtcNow).TotalSeconds;

        return new LoginResponse(identityId, accessToken, accessTokenExpiratesIn, refreshToken, refreshTokenExpiratesIn);
    }
}
