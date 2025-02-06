using AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed class GetAllDoctorsMappingProfile : Profile
{
    public GetAllDoctorsMappingProfile()
    {
        CreateMap<IEnumerable<Doctor>, GetAllDoctorsResponse>()
            .ForMember(dest => dest.Doctors, opt => opt.MapFrom(src => src));
        
        CreateMap<Doctor, DoctorResponse>()
            .ForMember(dest => dest.MedicalSpecialty, opt => opt.MapFrom(src => src.MedicalSpecialty));
        
        CreateMap<MedicalSpecialty, MedicalSpecialtyDto>();
    }
}

