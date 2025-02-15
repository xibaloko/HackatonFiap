﻿using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.AddPatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPatientsAsync(CancellationToken cancellationToken)
    {
        Result<GetAllPatientsResponse> response = await _mediator.Send(new GetAllPatientsRequest(), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpGet("{uuid:Guid}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientByUuidAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result<GetPatientByUuidResponse> response = await _mediator.Send(new GetPatientByUuidRequest(uuid), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddPatientAsync([FromBody] AddPatientRequest request, CancellationToken cancellationToken)
    {
        Result<AddPatientResponse> response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPut]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> UpdatePatientAsync([FromBody] UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpDelete("{uuid:Guid}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> DeletePatientAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(new DeletePatientRequest(uuid), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }
}
