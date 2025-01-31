using AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed class GetAllDoctorsMappingProfile : Profile
{

    public GetAllDoctorsMappingProfile()
    {
        CreateMap<IEnumerable<Doctor>, GetAllDoctorsResponse>()
            .ForPath(destination => destination.Doctors, opts => opts.MapFrom(source => source));

        CreateMap<Doctor, DoctorResponse>();
    }

}
