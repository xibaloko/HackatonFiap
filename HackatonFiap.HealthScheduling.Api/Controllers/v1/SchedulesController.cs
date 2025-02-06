using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddAppointmentSlot;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.RefuseSchedule;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.UpdateSchedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("generate-time-slots")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GenerateTimeSlotsAsync([FromBody] GenerateTimeSlotsRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AddAppointmentSlotAsync([FromBody] AddAppointmentSlotRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpGet("doctor/{uuid:Guid}")]
    [Authorize(Roles = "Doctor,Patient")]
    public async Task<IActionResult> GetScheduleFromDoctorAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetScheduleFromDoctorRequest(uuid), cancellationToken: cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPut]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> UpdateScheduleAsync([FromBody] UpdateScheduleRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken: cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPatch("refuse")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> RefuseScheduleAsync([FromBody] RefuseScheduleRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
}
