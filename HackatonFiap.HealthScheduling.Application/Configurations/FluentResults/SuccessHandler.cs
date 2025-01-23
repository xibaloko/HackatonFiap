using FluentResults;

namespace HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
public sealed class SuccessHandler : Success
{
    public static Success WithCustomHeaders(Dictionary<string, object> headers) => new HttpRequestSuccess(headers);
    public static Success WithCustomHeader(string name, object value) => new HttpRequestSuccess(name, value);
}
