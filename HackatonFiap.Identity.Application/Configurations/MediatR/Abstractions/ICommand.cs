using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.Configurations.MediatR.Abstractions;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}