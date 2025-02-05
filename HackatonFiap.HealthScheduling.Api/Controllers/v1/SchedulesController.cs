using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddAppointmentSlot;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GenerateTimeSlots;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("generate-time-slots")]
    public async Task<IActionResult> GenerateTimeSlots([FromBody] GenerateTimeSlotsRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
    
    [HttpPost("add-appointment-slot")]
    public async Task<IActionResult> AddAppointmentSlot([FromBody] AddAppointmentSlotRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }


    [HttpGet("doctor/{uuid:Guid}")]
    public async Task<IActionResult> GetScheduleFromDoctor([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetScheduleFromDoctorRequest(uuid), cancellationToken: cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpDelete("{uuid:Guid}")]
    public async Task<IActionResult> DeleteSchedule([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteScheduleRequest(uuid), cancellationToken: cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }



}
