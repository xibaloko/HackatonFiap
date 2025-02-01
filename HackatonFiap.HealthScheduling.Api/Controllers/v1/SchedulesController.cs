using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.AddSchedule;
using HackatonFiap.HealthScheduling.Application.UseCases.Schedules.GetScheduleFromDoctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("add-schedule")]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request, CancellationToken cancellationToken)
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

}
