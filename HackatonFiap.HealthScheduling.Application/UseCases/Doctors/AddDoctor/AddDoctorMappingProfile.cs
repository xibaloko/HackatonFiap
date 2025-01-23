using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorMappingProfile : ApplicationMappingProfile
{
    protected override void RegisterMappings()
    {
        CreateMap<AddDoctorRequest, Doctor>();
        CreateMap<Doctor, AddDoctorResponse>();
    }
}
