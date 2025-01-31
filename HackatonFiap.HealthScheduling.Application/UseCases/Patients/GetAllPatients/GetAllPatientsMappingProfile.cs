using AutoMapper;
using HackatonFiap.HealthScheduling.Application.Configurations.AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;

public class GetAllPatientsMappingProfile : Profile
{
    public GetAllPatientsMappingProfile()
    {
        CreateMap<IEnumerable<Patient>, GetAllPatientsResponse>()
            .ForPath(destination => destination.Patients, opts => opts.MapFrom(source => source));

        CreateMap<Patient, PatientResponse>();
    }
}
