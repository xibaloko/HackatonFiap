using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed class GetDoctorByUuidMappingProfile : ApplicationMappingProfile
{
    protected override void RegisterMappings()
    {
        CreateMap<Doctor, GetDoctorByUuidResponse>();
    }
}
