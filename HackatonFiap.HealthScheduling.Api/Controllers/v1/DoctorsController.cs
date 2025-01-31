using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{uuid:Guid}")]
    public async Task<IActionResult> GetDoctorByUuidAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result<GetDoctorByUuidResponse> response = await _mediator.Send(new GetDoctorByUuidRequest(uuid), cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpGet("getAll-doctors")]
    public async Task<IActionResult> GetAllDoctorsAsync(CancellationToken cancellationToken)
    {
        Result<GetAllDoctorsResponse> response = await _mediator.Send(new GetAllDoctorsRequest(), cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost("add-doctor")]
public async Task<IActionResult> AddDoctorAsync([FromBody] AddDoctorRequest request, CancellationToken cancellationToken)
{
    try
    {
        Result<AddDoctorResponse> response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
    catch (Exception ex)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Title = "Internal Server Error",
            Detail = ex.Message,
            Status = StatusCodes.Status500InternalServerError,
            Instance = HttpContext.Request.Path
        };

        return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
    }
}

}
