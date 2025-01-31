using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient
{
    internal class UpdatePatientMappingProfile : Profile
    {
        public UpdatePatientMappingProfile()
        {
            CreateMap<Patient, UpdatePatientResponse>();
        }
    }
}
