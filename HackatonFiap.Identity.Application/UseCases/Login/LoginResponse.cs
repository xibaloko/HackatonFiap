namespace HackatonFiap.Identity.Application.UseCases.Login;

public sealed class LoginResponse
{
    public Guid IdentityId { get; init; }
    public string AccessToken { get; init; }
    public int AccessTokenExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public int RefreshTokenExpiresIn { get; init; }

    public LoginResponse(Guid identityId, string accessToken, int accessTokenExpiresIn, string refreshToken, int refreshTokenExpiresIn)
    {
        IdentityId = identityId;
        AccessToken = accessToken;
        AccessTokenExpiresIn = accessTokenExpiresIn;
        RefreshToken = refreshToken;
        RefreshTokenExpiresIn = refreshTokenExpiresIn;
    }
}
