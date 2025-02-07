using FluentResults;
using HackatonFiap.Identity.Application.Configurations.ApiExtensions;
using HackatonFiap.Identity.Application.UseCases.CreateAccount;
using HackatonFiap.Identity.Application.UseCases.DeleteAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Identity.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        Result<CreateAccountResponse> response = await _mediator.Send(request, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }

    [HttpDelete("{identityId:Guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid identityId, CancellationToken cancellationToken)
    {
        Result<DeleteAccountResponse> response = await _mediator.Send(new DeleteAccountRequest { IdentityId = identityId }, cancellationToken);

        return this.ProcessResponse(response, cancellationToken);
    }
}
