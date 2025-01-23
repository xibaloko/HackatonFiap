using FluentValidation;

namespace HackatonFiap.Identity.Application.Configurations.FluentValidation;

public abstract class RequestValidator<T> : AbstractValidator<T>
{
    protected RequestValidator()
    {
        Validate();
    }
    protected abstract void Validate();
}
