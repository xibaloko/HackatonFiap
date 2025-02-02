namespace HackatonFiap.HealthScheduling.Domain.IdentityService;

public interface IApiIdentityService
{
    Task<Guid> CreateIdentity(string username, string email, string password, string role);
}
