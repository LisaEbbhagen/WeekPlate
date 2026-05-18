using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace AiRecipe.LlmProxy.Api.Extensions
     
{
    public static class RateLimitExtensions
    {
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services, IConfiguration config)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddSlidingWindowLimiter("sliding", opt =>
                {
                    opt.PermitLimit = 100;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.SegmentsPerWindow = 6; 
                    opt.QueueLimit = 5;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });
            return services;
        }
    }
}
