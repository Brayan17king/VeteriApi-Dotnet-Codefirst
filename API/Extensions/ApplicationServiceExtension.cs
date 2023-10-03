using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin() // WithOrigins("https://domain.com")
                .AllowAnyMethod() // WithMethods("GET", "POST")
                .AllowAnyHeader(); // WithHeaders("accept", "content-type")
            });
        }); // Remember to put 'static' on the class and to add builder.Services.AddApplicationServices(); to Program.cs and builder.Services.ConfigureCors(); and app.UseCors("CorsPolicy");

        public static void ConfigureRatelimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 2
                    }
                };
            });
        }
    }
}