using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.Configurations.MediatR.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{

}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
where TCommand : ICommand<TResponse>
{

}
