using AiRecipe.Content.Api.Clients;
using AiRecipe.Content.Api.Data;
using AiRecipe.Content.Api.Extensions;
using AiRecipe.Content.Api.Filters;
using AiRecipe.Content.Api.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Net.Http;


// Application startup
// Configures dependency injection, middleware pipeline,
// database connection, HTTP clients, caching, CORS, rate limiting,
// and global error handling.
var builder = WebApplication.CreateBuilder(args);

// 1. Register services (Dependency Injection)
// This prepares the application to handle incoming requests.
builder.Services.AddScoped<ExecutionTimeFilter>();
builder.Services.AddScoped<ApiKeyFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>(); // Apply the execution time filter globally
    options.Filters.Add<ApiKeyFilter>(); // Apply the API key filter globally
});

//Application services
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<ILlmClient, LlmClient>();

// HybridCache is preview in .NET 9, but stable enough for this use case.
// I suppress the warning intentionally.
#pragma warning disable EXTEXP0018 
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

//Scalar for documentation and testning
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

// 2. Database configuration
// Connects the API to SQL Server using EF Core.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RecipeDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. HTTP clients for external API
// Includes timeout and resilience (retry with exponential backoff).
var serviceSettingsLlmProxyBaseUrl = builder.Configuration["ServiceSettings:LlmProxyBaseUrl"] ?? throw new InvalidOperationException("LlmProxy Base URL is not configured.");

builder.Services.AddHttpClient<ILlmClient, LlmClient>(client =>
{
    client.BaseAddress = new Uri(serviceSettingsLlmProxyBaseUrl);
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
builder.Services.AddCustomCors(builder.Configuration);
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

    // 2. Scalar läser filen som Swagger skapade
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

// 7. Rate limiting + authorization + routing
app.UseRateLimiter(); 
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers(); // Map controller routes to the app

// 8. Start the application
app.Run();
