using FluentResults;
using HackatonFiap.Identity.Application.Configurations.ApiExtensions;
using HackatonFiap.Identity.Application.UseCases.CreateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Identity.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
        {
            Result response = await _mediator.Send(request, cancellationToken);

            return this.ProcessResponse(response, cancellationToken);
        }
    }
}
