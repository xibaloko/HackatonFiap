using AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;

namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public sealed class GetAllMedicalSpecialtiesMappingProfile : Profile
{

    public GetAllMedicalSpecialtiesMappingProfile()
    {
        CreateMap<IEnumerable<MedicalSpecialty>, GetAllMedicalSpecialtiesResponse>()
            .ForPath(destination => destination.MedicalSpecialties, opts => opts.MapFrom(source => source));

        CreateMap<MedicalSpecialty, MedicalSpecialtyResponse>();
    }

}
