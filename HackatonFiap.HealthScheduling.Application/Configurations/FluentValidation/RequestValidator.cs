using FluentValidation;

namespace HackatonFiap.HealthScheduling.Application.Configurations.FluentValidation;

public abstract class RequestValidator<T> : AbstractValidator<T>
{
    protected RequestValidator()
    {
        Validate();
    }
    protected abstract void Validate();
}
