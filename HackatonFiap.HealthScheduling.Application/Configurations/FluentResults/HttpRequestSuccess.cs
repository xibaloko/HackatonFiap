using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

internal sealed class HttpRequestSuccess : Success
{
    public HttpRequestSuccess(Dictionary<string, object> headers) => AppendSuccessMetadata(headers);

    public HttpRequestSuccess(string name, object value) => AppendSuccessMetadata(name, value);

    private void AppendSuccessMetadata(Dictionary<string, object> headers)
    {
        if (headers.Count > 0)
        {
            foreach (KeyValuePair<string, object> header in headers)
                WithMetadata(header.Key, header.Value);
        }
    }

    private void AppendSuccessMetadata(string name, object value) => WithMetadata(name, value);
}
