using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient
{
    internal class GetPatientMappingProfile : Profile
    {
        public GetPatientMappingProfile()
        {
            CreateMap<Patient, GetPatientResponse>();
        }
    }
}
