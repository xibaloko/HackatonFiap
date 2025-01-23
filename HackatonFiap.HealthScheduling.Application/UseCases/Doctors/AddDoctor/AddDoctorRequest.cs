using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequest : IRequest<Result<AddDoctorResponse>>
{
    public required string Name { get; init; }
    public required string LastName { get; init; }
}
