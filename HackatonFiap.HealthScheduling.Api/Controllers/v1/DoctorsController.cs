using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("add-doctor")]
    public async Task<IActionResult> AddDoctorAsync([FromBody] AddDoctorRequest request, CancellationToken cancellationToken)
    {
        Result<AddDoctorResponse> response = await _mediator.Send(request, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }
}
