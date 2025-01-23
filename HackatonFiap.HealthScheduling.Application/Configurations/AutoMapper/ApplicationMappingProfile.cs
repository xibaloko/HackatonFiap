using AutoMapper;

namespace HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;

public abstract class ApplicationMappingProfile : Profile
{
    protected ApplicationMappingProfile()
    {
        RegisterMappings();
    }
    protected abstract void RegisterMappings();
}