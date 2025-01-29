﻿using System.Threading;
using System.Threading.Tasks;
using HackatonFiap.HealthScheduling.Domain.Entities;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using AutoMapper; // Se você estiver usando AutoMapper, altere para AutoMapper
using MediatR;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient
{
    public sealed class GetPatientRequestHandler : IRequestHandler<GetPatientRequest, GetPatientResponse>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        // No construtor
        public GetPatientRequestHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<GetPatientResponse> Handle(GetPatientRequest request, CancellationToken cancellationToken)
        {
            var patient = await _patientRepository.GetByIdAsync(request.Uuid);

            if (patient == null)
                throw new KeyNotFoundException("Paciente não encontrado.");

            return _mapper.Map<GetPatientResponse>(patient);
        }
    }
}