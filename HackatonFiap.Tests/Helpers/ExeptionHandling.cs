using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Tests.Helpers;

public class ExeptionHandling
{
    public async Task<IActionResult> ExecuteWithExceptionHandling(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    public async Task<Result<T>> ExecuteWithExceptionHandling<T>(Func<Task<Result<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return Result.Fail<T>(ex.Message);
        }
    }

    public async Task<Result> ExecuteWithExceptionHandling(Func<Task<Result>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<IActionResult> ExecuteWithBadRequestHandling(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (ValidationException ex)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            };

            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }
        catch (Exception ex)
        {
            var error = new Error("400").WithMetadata("Message", ex.Message);
            var problemDetails = new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = error.Message,
                Status = StatusCodes.Status400BadRequest
            };

            foreach (var metadata in error.Metadata)
            {
                if (metadata.Value is string[] values)
                {
                    problemDetails.Errors.TryAdd(metadata.Key, values);
                }
            }

            return new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}
