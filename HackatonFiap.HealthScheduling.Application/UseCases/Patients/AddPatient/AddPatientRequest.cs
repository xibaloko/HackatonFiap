using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;

public sealed class AddPatientRequest : IRequest<Result<AddPatientResponse>>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CPF { get; init; }
    public required string RG { get; init; }
}
