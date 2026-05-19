using AiRecipe.Content.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AiRecipe.Content.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var problemDetails = exception switch
            {
                BadRequestException ex => new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = ex.Message
                },

                LlmClientUnauthorizedException ex => new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = ex.Message
                },

                NotFoundException ex => new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = ex.Message
                },

                LlmClientRateLimitException ex => new ProblemDetails
                {
                    Status = 429,
                    Title = "Too Many Requests",
                    Detail = ex.Message
                },

                LlmClientBadGatewayException ex => new ProblemDetails
                {
                    Status = 502,
                    Title = "AI Service Link Error",
                    Detail = ex.Message
                },

                LlmClientTimeOutException ex => new ProblemDetails
                {
                    Status = 504,
                    Title = "Llm Proxy Timeout",
                    Detail = ex.Message
                },

                // Fallback for unexpected errors
                _ => new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred. Please try again later."
                }
            };

            problemDetails.Instance = context.Request.Path;

            context.Response.StatusCode = problemDetails.Status ?? 500;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetails);
        }                            
    }
}
