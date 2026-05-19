using AiRecipe.LlmProxy.Api.Extensions;
using AiRecipe.LlmProxy.Api.Filters;
using AiRecipe.LlmProxy.Api.Services;
using OpenAI;
using Scalar.AspNetCore;


// Application startup
// Configures dependency injection, middleware pipeline,
// database connection, HTTP clients, caching, CORS, rate limiting,
// and global error handling.
var builder = WebApplication.CreateBuilder(args);

// 1. Register services (Dependency Injection)
// This prepares the application to handle incoming requests.
builder.Services.AddScoped<ApiKeyFilter>();
builder.Services.AddScoped<ExecutionTimeFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>(); // Apply the execution time filter globally
    options.Filters.Add<ApiKeyFilter>(); // Apply the API key filter globally
});

//Swagger/OpenAPI for documentation and testning
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Add XML-comments 
    var currentDirectory = AppContext.BaseDirectory;
    var xmlFiles = Directory.GetFiles(currentDirectory, "*.xml");

    foreach (var xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile);
    }

    // Definition for ApiKey
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "x-api-key",
        Description = "Write your internal API-key"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 3. HTTP clients for external APIs
// Includes timeout and resilience (retry with exponential backoff).
var openAIKey = builder.Configuration["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI Api key is not configured.");
var openAiBaseUrl = builder.Configuration["OpenAI:BaseUrl"] ?? throw new InvalidOperationException("OpenAI Base URL is not configured.");

builder.Services.AddHttpClient<ILlmService, LlmService>(client =>
{
    client.BaseAddress = new Uri(openAiBaseUrl);
    client.Timeout = TimeSpan.FromMinutes(2);
})
.AddStandardResilienceHandler(options =>
{
    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(3);
    options.AttemptTimeout.Timeout = TimeSpan.FromMinutes(2);

    options.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(4);

    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromSeconds(5);
    options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
});
// Custom CORS and Rate Limiting (extension methods)
//builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddCustomRateLimiter(builder.Configuration);

var app = builder.Build();

// 4. Custom global exception handling
// Converts thrown exceptions into clean ProblemDetails responses.
app.UseExceptionHandling();

// 5. Environment-specific configuration
// Swagger only in development, different CORS policies.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });

    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/openapi/v1.json");
    });
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

// 6. Security middleware
// HTTPS redirection + security headers.
app.UseHttpsRedirection();

// Adds common security headers to reduce attack surface
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    await next();
});

// 7. Rate limiting + authorization
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

