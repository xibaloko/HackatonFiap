using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HackatonFiap.Identity.Application.Configurations.ApiExtensions;

public static class ApiControllerExtensions
{
    public static ObjectResult ProcessResponse(this ControllerBase controller, Result result, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return controller.StatusCode(StatusCodes.Status499ClientClosedRequest, null);

        if (result.IsFailed)
            return controller.ProcessErrorResponse(result.Errors.Single());

        controller.TryAddCustomResponseHeaders(result);

        if (controller.HttpContext.Request.Method == HttpMethod.Post.ToString())
            return controller.StatusCode(StatusCodes.Status200OK, new());

        return controller.StatusCode(StatusCodes.Status204NoContent, null);
    }

    public static ObjectResult ProcessResponse<TResponse>(this ControllerBase controller, Result<TResponse> result, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return controller.StatusCode(StatusCodes.Status499ClientClosedRequest, null);

        if (result.IsFailed)
            return controller.ProcessErrorResponse(result.Errors.Single());

        return controller.StatusCode(StatusCodes.Status200OK, result.ValueOrDefault);
    }

    private static ObjectResult ProcessErrorResponse(this ControllerBase controller, IError error)
    {
        ValidationProblemDetails? problemDetails = controller.CreateProblemDetails(error);

        ArgumentNullException.ThrowIfNull(problemDetails);

        return controller.StatusCode(problemDetails!.Status!.Value, problemDetails);
    }

    private static ValidationProblemDetails? CreateProblemDetails(this ControllerBase controller, IError error)
    {
        IError? reason = error.Reasons.SingleOrDefault();

        if (reason is null) return default;

        int statusCode = int.Parse(reason.Message);

        string title = ReasonPhrases.GetReasonPhrase(statusCode);

        var problemDetails = new ValidationProblemDetails
        {
            Title = title,
            Detail = error.Message,
            Instance = controller.HttpContext.Request.Path,
            Status = statusCode,
        };

        foreach (KeyValuePair<string, object> errorMetadata in error.Metadata)
            problemDetails.Errors.TryAdd(errorMetadata.Key, (string[])errorMetadata.Value);

        return problemDetails;
    }

    private static void TryAddCustomResponseHeaders(this ControllerBase controller, Result result)
    {
        if (result.Successes.Count > 0)
        {
            foreach (ISuccess success in result.Successes)
            {
                if (success.Metadata.Count > 0)
                {
                    foreach (KeyValuePair<string, object> metadata in success.Metadata)
                        controller.HttpContext.Response.Headers.TryAdd(metadata.Key, metadata.Value.ToString());
                }
            }
        }
    }
}