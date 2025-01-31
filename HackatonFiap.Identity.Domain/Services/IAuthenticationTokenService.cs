namespace HackatonFiap.Identity.Domain.Services;

public interface IAuthenticationTokenService
{
    string GenerateAccessToken(string userId);
    string GenerateRefreshToken(string userId);
    (bool IsValid, string? Token) RenewAccessToken(string refreshToken);
    DateTime GetAccessTokenExpiration();
    DateTime GetRefreshTokenExpiration();
}
