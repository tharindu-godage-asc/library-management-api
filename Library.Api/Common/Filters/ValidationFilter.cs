using FluentValidation;

namespace Library.Api.Common.Filters;

public class ValidationFilter<T>
    : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator =
            context.HttpContext.RequestServices
                .GetService<IValidator<T>>();

        if (validator is null)
        {
            return await next(context);
        }

        var model =
            context.Arguments
                .OfType<T>()
                .FirstOrDefault();

        if (model is null)
        {
            return await next(context);
        }

        var validation =
            await validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
            return Results.BadRequest(new
            {
                statusCode = 400,
                message = "Validation failed",
                errors = validation.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                })
            });
        }

        return await next(context);
    }
}