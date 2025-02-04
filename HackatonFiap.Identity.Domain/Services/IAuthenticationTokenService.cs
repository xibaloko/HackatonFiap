namespace HackatonFiap.Identity.Domain.Services;

public interface IAuthenticationTokenService
{
    string GenerateAccessToken(string userId, IEnumerable<string> roles);
    string GenerateRefreshToken(string userId, IEnumerable<string> roles);
    (bool IsValid, string? Token) RenewAccessToken(string refreshToken);
    DateTime GetAccessTokenExpiration();
    DateTime GetRefreshTokenExpiration();
}
