using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;

public sealed record GetAllDoctorsRequest : IRequest<Result<GetAllDoctorsResponse>>
{
    public string? Specialty { get; }

    public GetAllDoctorsRequest(string? specialty = null)
    {
        Specialty = specialty;
    }
}
