using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public sealed record UpdatePatientRequest(
    Guid Uuid,
    string Name, 
    string LastName, 
    string Email, 
    string CPF, 
    string RG) : IRequest<Result>
{
}
