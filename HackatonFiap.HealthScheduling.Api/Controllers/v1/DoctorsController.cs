using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.UpdateDoctor;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.DeleteDoctor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Doctor")]
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

    [HttpGet]
    public async Task<IActionResult> GetAllDoctorsAsync(CancellationToken cancellationToken)
    {
        Result<GetAllDoctorsResponse> response = await _mediator.Send(new GetAllDoctorsRequest(), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddDoctorAsync([FromBody] AddDoctorRequest request, CancellationToken cancellationToken)
    {
        Result<AddDoctorResponse> response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDoctorAsync([FromBody] UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(request, cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpDelete("{uuid:Guid}")]
    public async Task<IActionResult> DeleteDoctorAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        Result response = await _mediator.Send(new DeleteDoctorRequest(uuid), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

}
