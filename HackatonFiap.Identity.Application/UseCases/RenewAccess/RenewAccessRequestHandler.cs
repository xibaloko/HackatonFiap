using FluentResults;
using HackatonFiap.Identity.Application.Configurations.FluentResults;
using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HackatonFiap.Identity.Application.UseCases.RenewAccess;

public sealed class RenewAccessRequestHandler : IRequestHandler<RenewAccessRequest, Result<RenewAccessResponse>>
{
    private readonly IAuthenticationTokenService _authenticationTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RenewAccessRequestHandler(
        IAuthenticationTokenService authenticationTokenService,
        UserManager<ApplicationUser> userManager
        )
    {
        _authenticationTokenService = authenticationTokenService;
        _userManager = userManager;
    }

    public async Task<Result<RenewAccessResponse>> Handle(RenewAccessRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email).WaitAsync(cancellationToken);

        if (user is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Invalid credentials."));

        var (IsValid, Token) = _authenticationTokenService.RenewAccessToken(request.RefreshToken);

        if (!IsValid)
            return Result.Fail(ErrorHandler.HandleUnauthorized("Invalid token."));

        RenewAccessResponse response = GenerateResponse(user, Token!);

        return Result.Ok(response);
    }

    private RenewAccessResponse GenerateResponse(ApplicationUser user, string newAccessToken)
    {
        Guid identityId = Guid.Parse(user.Id);
        int accessTokenExpiratesIn = (int)(_authenticationTokenService.GetAccessTokenExpiration() - DateTime.UtcNow).TotalSeconds;
        string newRefreshToken = _authenticationTokenService.GenerateRefreshToken(user.UserName!);
        int refreshTokenExpiratesIn = (int)(_authenticationTokenService.GetRefreshTokenExpiration() - DateTime.UtcNow).TotalSeconds;

        return new RenewAccessResponse(identityId, newAccessToken, accessTokenExpiratesIn, newRefreshToken, refreshTokenExpiratesIn);
    }
}
