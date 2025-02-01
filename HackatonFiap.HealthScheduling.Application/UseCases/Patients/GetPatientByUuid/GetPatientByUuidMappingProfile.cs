using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;

internal class GetPatientByUuidMappingProfile : Profile
{
    public GetPatientByUuidMappingProfile()
    {
        CreateMap<Patient, GetPatientByUuidResponse>();
    }
}
