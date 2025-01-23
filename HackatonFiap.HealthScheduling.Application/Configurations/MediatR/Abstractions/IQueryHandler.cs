using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.Configurations.MediatR.Abstractions;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{

}
