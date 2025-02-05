using MediatR;
using AutoMapper;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public class GetAllDoctorsRequestHandler : IRequestHandler<GetAllDoctorsRequest, Result<GetAllDoctorsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDoctorsRequestHandler(IUnitOfWork repositories, IMapper mapper)
    {
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetAllDoctorsResponse>> Handle(GetAllDoctorsRequest request, CancellationToken cancellationToken)
    {
        var doctors = await _unitOfWork.DoctorRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = _mapper.Map<GetAllDoctorsResponse>(doctors);

        return Result.Ok(response);
    }
}