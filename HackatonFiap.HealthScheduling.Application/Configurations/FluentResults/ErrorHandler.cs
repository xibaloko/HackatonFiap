using FluentResults;
using Microsoft.AspNetCore.Http;

namespace HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;

public sealed class ErrorHandler : Error
{
    public static Error HandleBadRequest(string message) => new HttpRequestError(StatusCodes.Status400BadRequest, message);
    public static Error HandleBadRequest(Dictionary<string, string[]> errors) => new HttpRequestError(StatusCodes.Status400BadRequest, errors);
    public static Error HandleUnauthorized(string message) => new HttpRequestError(StatusCodes.Status401Unauthorized, message);
    public static Error HandlePaymentRequired(string message) => new HttpRequestError(StatusCodes.Status402PaymentRequired, message);
    public static Error HandleForbidden(string message) => new HttpRequestError(StatusCodes.Status403Forbidden, message);
    public static Error HandleNotFound(string message) => new HttpRequestError(StatusCodes.Status404NotFound, message);
    public static Error HandleMethodNotAllowed(string message) => new HttpRequestError(StatusCodes.Status405MethodNotAllowed, message);
    public static Error HandleNotAcceptable(string message) => new HttpRequestError(StatusCodes.Status406NotAcceptable, message);
    public static Error HandleProxyAuthenticationRequired(string message) => new HttpRequestError(StatusCodes.Status407ProxyAuthenticationRequired, message);
    public static Error HandleRequestTimeout(string message) => new HttpRequestError(StatusCodes.Status408RequestTimeout, message);
    public static Error HandleConflict(string message) => new HttpRequestError(StatusCodes.Status409Conflict, message);
    public static Error HandleLengthRequired(string message) => new HttpRequestError(StatusCodes.Status411LengthRequired, message);
    public static Error HandlePreConditionFailed(string message) => new HttpRequestError(StatusCodes.Status412PreconditionFailed, message);
    public static Error HandlePayloadTooLarge(string message) => new HttpRequestError(StatusCodes.Status413PayloadTooLarge, message);
    public static Error HandleUriTooLong(string message) => new HttpRequestError(StatusCodes.Status414RequestUriTooLong, message);
    public static Error HandleUnsupportedMediaType(string message) => new HttpRequestError(StatusCodes.Status415UnsupportedMediaType, message);
    public static Error HandleFailedDependency(string message) => new HttpRequestError(StatusCodes.Status424FailedDependency, message);
    public static Error HandleTooManyRequests(string message) => new HttpRequestError(StatusCodes.Status429TooManyRequests, message);
    public static Error HandleUnavailableForLegalReasons(string message) => new HttpRequestError(StatusCodes.Status451UnavailableForLegalReasons, message);
    public static Error HandleClientClosedRequest(string message) => new HttpRequestError(StatusCodes.Status499ClientClosedRequest, message);
    public static Error HandleInternalServerError(string message) => new HttpRequestError(StatusCodes.Status500InternalServerError, message);
    public static Error HandleNotImplemented(string message) => new HttpRequestError(StatusCodes.Status501NotImplemented, message);
    public static Error HandleBadGateway(string message) => new HttpRequestError(StatusCodes.Status502BadGateway, message);
    public static Error HandleServiceUnavailable(string message) => new HttpRequestError(StatusCodes.Status503ServiceUnavailable, message);
    public static Error HandleGatewayTimeout(string message) => new HttpRequestError(StatusCodes.Status504GatewayTimeout, message);
    public static Error HandleHttpVersionNotSupported(string message) => new HttpRequestError(StatusCodes.Status505HttpVersionNotsupported, message);
    public static Error HandleNetworkAuthenticationRequired(string message) => new HttpRequestError(StatusCodes.Status511NetworkAuthenticationRequired, message);
}
