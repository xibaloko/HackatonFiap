using FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.Configurations.MediatR.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
