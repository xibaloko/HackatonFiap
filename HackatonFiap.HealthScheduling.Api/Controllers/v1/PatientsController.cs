using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("getAll-patients")]
    public async Task<IActionResult> GetAllPatientsAsync(CancellationToken cancellationToken)
    {
        try
        {
            Result<UpdatePatientResponse> response = await _mediator.Send(new UpdatePatientRequest(), cancellationToken);
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

    [HttpPost("add-patient")]
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


    /// <summary>
    /// Atualiza um paciente pelo UUID.
    /// </summary>
    [HttpPut("{uuid}")]
    public async Task<IActionResult> UpdatePatient(Guid Uuid, [FromBody] UpdatePatientRequest request)
    {
        try
        {
            var result = await _mediator.Send(request);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);

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

    /// <summary>
    /// Deleta um paciente pelo UUID.
    /// </summary>
    [HttpDelete("{uuid}")]
    public async Task<IActionResult> DeletePatient(Guid Uuid)
    {
        try 
        { 
            var result = await _mediator.Send(new DeletePatientRequest(Uuid));

            if (result.IsFailed)
            {
                return NotFound(result.Errors);
            }

            return NoContent();
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
