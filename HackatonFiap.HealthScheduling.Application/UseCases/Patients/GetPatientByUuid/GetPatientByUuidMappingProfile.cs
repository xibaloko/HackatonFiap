using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid
{
    internal class GetPatientByUuidMappingProfile : Profile
    {
        public GetPatientByUuidMappingProfile()
        {
            CreateMap<Patient, GetPatientByUuidResponse>();
        }
    }
}
