using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public class GetAllDoctorsRequestHandler : IRequestHandler<GetAllDoctorsRequest, List<GetAllDoctorsResponse>>
{
    private readonly IDoctorRepository _DoctorRepository;
    private readonly IMapper _mapper;

    // No construtor
    public GetAllDoctorsRequestHandler(IDoctorRepository DoctorRepository, IMapper mapper)
    {
        _DoctorRepository = DoctorRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllDoctorsResponse>> Handle(GetAllDoctorsRequest request, CancellationToken cancellationToken)
    {
        var Doctors = await _DoctorRepository.GetAllAsync();

        return _mapper.Map<List<GetAllDoctorsResponse>>(Doctors);
    }
}