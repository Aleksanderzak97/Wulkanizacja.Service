using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Wulkanizacja.Service.Api.Exceptions;
using Wulkanizacja.Service.Core.Exceptions;
using Wulkanizacja.Service.Infrastructure.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ApiExceptions => (int)HttpStatusCode.BadRequest,
            InfrastructureException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var errorResponse = new ErrorResponse
        {
            Code = (exception as InfrastructureException)?.Code ?? (exception as ApiExceptions)?.Code ?? "internal_server_error",
            Reason = exception.Message
        };

        var result = JsonConvert.SerializeObject(errorResponse);

        return context.Response.WriteAsync(result);
    }
}
