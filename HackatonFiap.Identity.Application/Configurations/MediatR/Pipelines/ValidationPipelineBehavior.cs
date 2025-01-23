using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using HackatonFiap.Identity.Application.Configurations.FluentResults;
using MediatR;

namespace HackatonFiap.Identity.Application.Configurations.MediatR.Pipelines;

internal sealed class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : ResultBase<TResponse>, new()
{
    private readonly IValidator<TRequest> _validator;
    public ValidationPipelineBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, options: options =>
        {
            options.IncludeAllRuleSets();
        }, cancellationToken);

        if (!validationResult.IsValid)
        {
            Dictionary<string, string[]> errorMetadata = BuildErrorMetadata(validationResult.Errors);

            var invalidRequest = new TResponse();

            invalidRequest.WithReason(ErrorHandler.HandleBadRequest(errorMetadata));

            return invalidRequest;
        }

        return await next();
    }

    private static Dictionary<string, string[]> BuildErrorMetadata(List<ValidationFailure> errors)
    {
        if (errors?.Count > 0)
        {
            return errors.GroupBy(error => error.PropertyName)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(e => e.ErrorMessage).ToArray());
        }

        return default!;
    }
}