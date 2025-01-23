using FluentResults;

namespace HackatonFiap.Identity.Application.Configurations.FluentResults;

internal sealed class HttpRequestError : Error
{
    public HttpRequestError(int statusCode, string message) => AppendErrorMessage(statusCode.ToString(), message);

    public HttpRequestError(int statusCode, Dictionary<string, string[]> errors) => AppendErrorMetadata(statusCode.ToString(), errors);

    private void AppendErrorMessage(string statusCode, string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            Message = message;

            CausedBy(statusCode);
        }
    }

    private void AppendErrorMetadata(string statusCode, Dictionary<string, string[]> errors)
    {
        if (errors?.Count > 0)
        {
            foreach (KeyValuePair<string, string[]> error in errors)
                WithMetadata(error.Key, error.Value);

            Message = "One or more validation errors occurred";

            CausedBy(statusCode);
        }
    }
}