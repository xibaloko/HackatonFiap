using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed record GetDoctorByUuidRequest(Guid Uuid) : IRequest<Result<GetDoctorByUuidResponse>>
{
    
}
