using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public class GetAllDoctorsRequestHandler : IRequestHandler<GetAllDoctorsRequest, Result<GetAllDoctorsResponse>>
{
    private readonly IRepositories _repositories;
    private readonly IMapper _mapper;

    public GetAllDoctorsRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllDoctorsResponse>> Handle(GetAllDoctorsRequest request, CancellationToken cancellationToken)
    {
        var doctors = await _repositories.DoctorRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllDoctorsResponse>(doctors);

        return Result.Ok(response);
    }
}