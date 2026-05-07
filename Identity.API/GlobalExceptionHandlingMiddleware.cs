using System.Text.Json;

namespace Identity.API;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(
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
        catch (FluentValidation.ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleValidationException(
        HttpContext context,
        FluentValidation.ValidationException exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Validation failed",
            Errors = exception.Errors
                .Select(x => new
                {
                    Property = x.PropertyName,
                    Error = x.ErrorMessage
                })
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private static async Task HandleException(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}