﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients
{
    public class GetAllPatientsResponse
    {
        public Guid Uuid { get; init; }
        public required string Name { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string CPF { get; init; }
        public required string RG { get; init; }
    }
}
