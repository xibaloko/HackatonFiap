using FluentResults;
using HackatonFiap.Identity.Application.Configurations.ApiExtensions;
using HackatonFiap.Identity.Application.UseCases.Login;
using HackatonFiap.Identity.Application.UseCases.RenewAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Identity.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        Result<LoginResponse> response = await _mediator.Send(request, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpPost("renew-access")]
    public async Task<IActionResult> RenewAccessAsync([FromBody] RenewAccessRequest request, CancellationToken cancellationToken)
    {
        Result<RenewAccessResponse> response = await _mediator.Send(request, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }
}
