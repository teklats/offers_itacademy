using System.Net;
using Newtonsoft.Json;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex)
                .ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var error = new ApiError(context, ex);

        var result = JsonConvert.SerializeObject(error);

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.Status;

        await context.Response
            .WriteAsync(result)
            .ConfigureAwait(false);
    }

    public class ApiError
    {
        public string Code;
        public string Title;
        public int Status;
        public string TraceId;
        public string Instance;

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

        private void HandleException(ForbiddenException exception)
        {
            Code = exception.Code;
            Status = (int)HttpStatusCode.Forbidden;
            Title = exception.Message;
        }

    }
}
