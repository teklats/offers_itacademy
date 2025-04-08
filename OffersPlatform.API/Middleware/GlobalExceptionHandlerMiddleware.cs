using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var error = new ApiError(context, ex);

        var result = JsonConvert.SerializeObject(error);

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.Status;

        await context.Response.WriteAsync(result);
    }

    public class ApiError
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public string Instance { get; set; }
        
        public ApiError(HttpContext httpContext, Exception exception)
        {
            TraceId = httpContext.TraceIdentifier;
            Instance = httpContext.Request.Path;

            Status = (int)HttpStatusCode.InternalServerError;
            Code = "SERVER_ERROR";
            Title = "An unexpected error occurred.";

            if (exception is NotFoundException notFoundEx)
                HandleException(notFoundEx);
            else if (exception is AlreadyExistsException alreadyExistsEx)
                HandleException(alreadyExistsEx);
            else if (exception is BadRequestException badRequestEx)
                HandleException(badRequestEx);
            else if (exception is UnauthorizedException unauthorizedEx)
                HandleException(unauthorizedEx);
            else
                Title = exception.Message;
        }

        private void HandleException(NotFoundException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.NotFound;
            Title = exception.Message;
        }

        private void HandleException(AlreadyExistsException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Conflict;
            Title = exception.Message;
        }
        
        private void HandleException(BadRequestException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.BadRequest;
            Title = exception.Message;
        }

        private void HandleException(UnauthorizedException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Unauthorized;
            Title = exception.Message;
        }
        
    }
}