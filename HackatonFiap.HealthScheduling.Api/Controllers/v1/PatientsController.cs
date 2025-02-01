using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAllPatientsAsync(CancellationToken cancellationToken)
    {
        try
        {
            Result<GetAllPatientsResponse> response = await _mediator.Send(new GetAllPatientsRequest(), cancellationToken);
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

    [HttpGet("{uuid:Guid}")]
    public async Task<IActionResult> GetPatientByUuidAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            Result<GetPatientByUuidResponse> response = await _mediator.Send(new GetPatientByUuidRequest(uuid), cancellationToken);
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

    [HttpPost]
    public async Task<IActionResult> AddPatientAsync([FromBody] AddPatientRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Result<AddPatientResponse> response = await _mediator.Send(request, cancellationToken);
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

    [HttpPut]
    public async Task<IActionResult> UpdatePatientAsync([FromBody] UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Result response = await _mediator.Send(request, cancellationToken);
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

    [HttpDelete("{uuid:Guid}")]
    public async Task<IActionResult> DeletePatientAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        try
        {
            Result response = await _mediator.Send(new DeletePatientRequest(uuid), cancellationToken);
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
