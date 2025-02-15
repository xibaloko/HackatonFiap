﻿using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.RefuseAppointment;

public sealed record RefuseAppointmentRequest(
    Guid AppointmentUuid,
    string CancellationReason
    ) : IRequest<Result>
{
}
