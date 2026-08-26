namespace AiRecipe.Content.Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                // Policy for production
                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("https://myRecipeApp.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                // Policy for development
                options.AddPolicy("DevelopmentPolicy", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173" // Frontend Vite HTTPS 
                        //"https://localhost:7121", // Service B HTTPS
                        //"http://localhost:5218"   // Service B HTTP
                    )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            return services;
        }
    }
}
