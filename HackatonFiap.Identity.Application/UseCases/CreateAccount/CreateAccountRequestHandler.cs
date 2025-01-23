using AutoMapper;
using FluentResults;
using HackatonFiap.Identity.Application.Configurations.FluentResults;
using HackatonFiap.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HackatonFiap.Identity.Application.UseCases.CreateAccount;

public sealed class CreateAccountRequestHandler : IRequestHandler<CreateAccountRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CreateAccountRequestHandler(
        UserManager<ApplicationUser> userManager, 
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        ApplicationUser user = _mapper.Map<ApplicationUser>(request);

        var result = await _userManager.CreateAsync(user, request.Password).WaitAsync(cancellationToken);

        if (!result.Succeeded)
            return Result.Fail(ErrorHandler.HandleBadRequest("Something went wrong."));

        return Result.Ok();
    }
}
