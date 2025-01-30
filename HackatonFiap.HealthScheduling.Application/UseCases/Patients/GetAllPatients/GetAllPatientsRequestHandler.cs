using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients
{
    public class GetAllPatientsRequestHandler : IRequestHandler<GetAllPatientsRequest, List<GetAllPatientsResponse>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        // No construtor
        public GetAllPatientsRequestHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAllPatientsResponse>> Handle(GetAllPatientsRequest request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAllAsync();

            return _mapper.Map<List<GetAllPatientsResponse>>(patients);
        }
    }
}
