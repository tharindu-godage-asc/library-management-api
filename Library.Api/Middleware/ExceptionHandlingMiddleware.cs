using Library.Api.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Library.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;

        switch (exception)
        {
            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case BusinessRuleException:
                statusCode = StatusCodes.Status400BadRequest;
                break;

            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                break;

            case ConflictException:
                statusCode = StatusCodes.Status409Conflict;
                break;
        }

        var response = new
        {
            statusCode,
            message = exception.Message,
            traceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}