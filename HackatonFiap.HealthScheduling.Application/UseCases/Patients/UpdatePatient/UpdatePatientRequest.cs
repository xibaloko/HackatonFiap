using FluentResults;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public sealed record UpdatePatientRequest(Guid Uuid) : IRequest<Result<UpdatePatientResponse>>
{
    public Guid Uuid { get; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string CPF { get; set; }
    public required string RG { get; set; }
}
