using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.ApiExtensions;
using HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetAllDoctors;
using HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.HealthScheduling.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MedicalSpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicalSpecialtiesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllMedicalSpecialtiesAsync(CancellationToken cancellationToken)
    {
        Result<GetAllMedicalSpecialtiesResponse> response = await _mediator.Send(new GetAllMedicalSpecialtiesRequest(), cancellationToken);
        return this.ProcessResponse(response, cancellationToken);
    }

}
