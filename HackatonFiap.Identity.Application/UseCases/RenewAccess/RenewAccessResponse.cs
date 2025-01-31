namespace HackatonFiap.Identity.Application.UseCases.RenewAccess;

public sealed class RenewAccessResponse
{
    public Guid IdentityId { get; init; }
    public string AccessToken { get; init; }
    public int AccessTokenExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public int RefreshTokenExpiresIn { get; init; }

    public RenewAccessResponse(Guid identityId, string accessToken, int accessTokenExpiresIn, string refreshToken, int refreshTokenExpiresIn)
    {
        IdentityId = identityId;
        AccessToken = accessToken;
        AccessTokenExpiresIn = accessTokenExpiresIn;
        RefreshToken = refreshToken;
        RefreshTokenExpiresIn = refreshTokenExpiresIn;
    }
}
