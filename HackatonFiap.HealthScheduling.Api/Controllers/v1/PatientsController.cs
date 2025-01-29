using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("add-patient")]
    public async Task<IActionResult> AddPatientAsync([FromBody] AddPatientRequest request, CancellationToken cancellationToken)
    {
        Result<AddPatientResponse> response = await _mediator.Send(request, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(Guid id)
    {
        var request = new GetPatientRequest { Uuid = id };
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetAllPatientsRequest());
        return Ok(response);
    }
}
