using AutoMapper;
using FluentResults;
using HackatonFiap.Identity.Application.Configurations.FluentResults;
using HackatonFiap.Identity.Application.UseCases.CreateAccount;
using HackatonFiap.Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HackatonFiap.Identity.Application.UseCases.DeleteAccount;

public sealed class DeleteAccountRequestHandler : IRequestHandler<DeleteAccountRequest, Result<DeleteAccountResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteAccountRequestHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<DeleteAccountResponse>> Handle(DeleteAccountRequest request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.IdentityId.ToString()).WaitAsync(cancellationToken);
        if (user is null)
            return Result.Fail(ErrorHandler.HandleNotFound("User not found."));

        var result = await _userManager.DeleteAsync(user).WaitAsync(cancellationToken);

        if (!result.Succeeded)
            return Result.Fail(ErrorHandler.HandleBadRequest("It was not possible to remove account."));

        return Result.Ok(new DeleteAccountResponse(user.Id));
    }
}
