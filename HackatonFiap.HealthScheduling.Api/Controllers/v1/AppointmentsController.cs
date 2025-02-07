using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Appointments.CreateAppointment;
using HackatonFiap.HealthScheduling.Application.UseCases.Appointments.RefuseAppointment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetPatientAppointments;
using HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetDoctorAppointments;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
    
    [HttpGet("patient/{uuid:Guid}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientAppointmentsAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result<GetPatientAppointmentsResponse> response = await _mediator.Send(new GetPatientAppointmentsRequest(uuid), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpGet("doctor/{uuid:Guid}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorAppointmentsAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result<GetDoctorAppointmentsResponse> response = await _mediator.Send(new GetDoctorAppointmentsRequest(uuid), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost("refuse")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> RefuseAppointmentAsync([FromBody] RefuseAppointmentRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
}
