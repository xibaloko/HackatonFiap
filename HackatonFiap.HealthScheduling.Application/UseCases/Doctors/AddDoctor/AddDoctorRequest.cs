﻿using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorRequest : IRequest<Result<AddDoctorResponse>>
{
    public required string CRM { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required string CPF { get; init; }
    public Guid MedicalSpecialtyUuid { get; init; }
}
