using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HackatonFiap.Identity.Domain.Services;

namespace HackatonFiap.Identity.Application.Services;

public class AuthenticationTokenService : IAuthenticationTokenService
{
    private readonly IConfiguration _configuration;
    private readonly double _accessTokenExpirationMinutes;
    private readonly double _refreshTokenExpirationMinutes;

    public AuthenticationTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _accessTokenExpirationMinutes = double.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!);
        _refreshTokenExpirationMinutes = double.Parse(_configuration["Jwt:RefreshTokenExpirationMinutes"]!);
    }

    public string GenerateAccessToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessTokenKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("TokenType", "access-token")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshTokenKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("TokenType", "refresh-token")
        };

        var refreshToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(refreshToken);
    }

    public (bool IsValid, string? Token) RenewAccessToken(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshTokenKey"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(refreshToken, validationParameters, out _);
            var username = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
                return (false, null);

            var newAccessToken = GenerateAccessToken(username);
            return (true, newAccessToken);
        }
        catch (Exception)
        {
            return (false, null);
        }
    }

    public DateTime GetAccessTokenExpiration() => DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

    public DateTime GetRefreshTokenExpiration() => DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes);
}
