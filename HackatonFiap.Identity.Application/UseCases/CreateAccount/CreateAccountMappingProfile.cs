using HackatonFiap.Identity.Application.Configurations.AutoMapper;
using HackatonFiap.Identity.Domain.Entities;

namespace HackatonFiap.Identity.Application.UseCases.CreateAccount;

public sealed class CreateAccountMappingProfile : ApplicationMappingProfile
{
    protected override void RegisterMappings()
    {
        CreateMap<CreateAccountRequest, ApplicationUser>();
    }
}
