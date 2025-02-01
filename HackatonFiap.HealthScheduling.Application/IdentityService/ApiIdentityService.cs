using HackatonFiap.HealthScheduling.Domain.IdentityService;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace HackatonFiap.HealthScheduling.Application.IdentityService;

public sealed class ApiIdentityService : IApiIdentityService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private const string identityApiBaseUrl = "IdentityApiBaseUrl";
    public ApiIdentityService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }
    public async Task<Guid> CreateIdentity(string username, string email, string password, string role)
    {
        var client = _httpClientFactory.CreateClient();

        client.BaseAddress = new Uri(_configuration[identityApiBaseUrl]!);

        var request = new AccountRequest
        {
            Role = role,
            Username = username,
            Email = email,
            Password = password
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("accept", "*/*");
        content.Headers.Add("x-api-version", "1");

        var response = await client.PostAsync("api/v1/Accounts/create", content);

        if (!response.IsSuccessStatusCode)
            return Guid.Empty;

        var responseContent = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseContent);
        var identityId = document.RootElement.GetProperty("identityId").GetGuid();
     
        return identityId;
    }
}

public class AccountRequest
{
    public required string Role { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}