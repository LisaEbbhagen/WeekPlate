# Content assistant API - Distributed Microservices Solution

A robust content assitant API built with .NET 9 and SQL Server. The goal of this project was to move beyond basic CRUD by implementing structured backend patterns such as external LLM API synchronization, smart caching, and partial update logic (PATCH). I focused on creating an API that is predictable for developers and high-performing for the end-user.


## Features
- **Microservice Solution:** This is a microservice solution with internal and external integration. 
- **AI-integration:** Through user prompt, AI provides the user with a weekly menu of her preferences.
- **API-key filter:** With internal api key we level up the security and only proceed if the right api key is included.
- **Custom middleware:** Custom made exception handling middleware. 
- **Partial Updates:** PATCH implementation using nullable DTOs to ensure only provided fields are modified.
- **HybridCache:** Implemented a multi-tier caching strategy (L1/L2) with built-in protection against cache stampedes. This ensures that even under high load, the database is only queried once to populate the cache.

## Tech Stack
- .NET 9 (ASP.NET Core)
- Entity Framework Core (SQL Server)
- Scalar / OpenAPI
- User Secrets for secure configuration


## Run the project (locally/develop)
1. **Internal API Key:** This project has an internal api key for secure integration between internal services. 
   **External API Key:** This project integrates with Open AI Platform to generate weekly mealplans. To run it locally, you need to create an account and obtain API key:
  - [Open AI API](https://platform.openai.com/login)

2. **Configure User Secrets:** Open your terminal in the project root and run:
```
dotnet user-secrets set "OpenAI:ApiKey" "YOUR_OPENAI_API_KEY"
dotnet user-secrets set "ServiceSettings:InternalApiKey" "YOUR_INTERNAL_API_KEY"
```
3. **Database:** Apply migrations to create your local database:
`dotnet ef database update`

4. **Start:** Run the application:
`dotnet run`
Access the Scalar documentation at `https://localhost:7148/scalar/v1` (check your terminal for the specific port).

**Note on Multiple Startup:** Since this is a microservice solution, ensure both AiRecipe.Content.Api and AiRecipe.LlmProxy.Api are running simultaneously. In Visual Studio, right-click the Solution ➔ "Configure Startup Projects" ➔ "Multiple startup projects"

## Security & API Key Management
* **Locally:** To secure the OpenAI API key during development, **.NET User Secrets** have been used. This guarantees that no secrets are accidentally committed to GitHub or stored in `appsettings.json`.
* **In Production:** In a production environment, this configuration should be replaced by using **Environment Variables (env vars)** or securely injected via cloud providers (such as *Azure Key Vault* or GitHub Secrets) for centralized and secure management without leaking any credentials into repositories or logs.

## Custom Exception Middleware
I have implemented service-specific exceptions to maintain a clear separation of concerns. Service B utilizes LlmProxyException for external provider errors, while Service A employs LlmClientException for internal proxy communication issues. 

The middleware automatically transforms exceptions into application/problem+json responses (RFC 7807).

**HTTP Mapping:**
- NotFoundException ➔ 404 Not Found
- BadRequestException ➔ 400 Bad Request
- LlmProxyException ➔ 502 Bad Gateway
- LlmClientException ➔ 502 Bad Gateway
- Unhandled Exceptions ➔ 500 Internal Server Error (Sanitized to prevent sensitive data leaks)

**Testing the Custom Middleware:** 
To verify the middleware, switch to a Single Startup project (running only AiRecipe.Content.Api). Attempting to use the GenerateWeeklyMealPlan endpoint will trigger a connection failure since the proxy is offline. The middleware intercepts this HttpRequestException (wrapped as an LlmClientException), returning a structured 502 Bad Gateway response as seen below:
<img width="851" height="318" alt="image" src="https://github.com/user-attachments/assets/7fef9315-5133-4786-8186-5d1c7f410b54" />

## AI Model Evaluation 
I have evaluated the outputs across three iterations to see how prompt engineering changes the quality of the data. To read the full version, click [here](docs/evaluation.md).

