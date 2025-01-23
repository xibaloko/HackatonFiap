using FluentResults;
using MediatR.Pipeline;
using MediatR;
using Microsoft.Extensions.Logging;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Exceptions;

namespace HackatonFiap.HealthScheduling.Application.Configurations.MediatR.ExceptionHandling;

internal sealed class DomainExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
where TRequest : IRequest<TResponse>
where TResponse : ResultBase<TResponse>, new()
where TException : DomainException
{
    private readonly ILogger<DomainExceptionHandler<TRequest, TResponse, TException>> _logger;
    public DomainExceptionHandler(ILogger<DomainExceptionHandler<TRequest, TResponse, TException>> logger) => _logger = logger;

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        TException excpetionResume = exception;

        _logger.LogTrace(excpetionResume, "Domain expection while handling request of type {@requestType}", typeof(TRequest));

        var response = new TResponse();

        response.WithReason(ErrorHandler.HandleBadRequest(excpetionResume.Message));

        state.SetHandled(response);

        return Task.CompletedTask;
    }
}