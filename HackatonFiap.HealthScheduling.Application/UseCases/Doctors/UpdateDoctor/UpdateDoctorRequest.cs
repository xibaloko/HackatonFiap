using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;

public sealed record UpdateDoctorRequest(
    Guid Uuid,
    string Name, 
    string LastName, 
    string Email, 
    string CPF, 
    string CRM) : IRequest<Result>
{
}
