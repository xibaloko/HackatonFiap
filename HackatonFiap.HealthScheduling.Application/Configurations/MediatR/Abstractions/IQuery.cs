using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.Configurations.MediatR.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
