using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.UnitOfWork;

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

        //agregar la unidad de trabajo al scoope
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

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
                        Endpoint = "*",//todos los enpoints
                        Period = "10s", //peticiones cada 10 segundos
                        Limit = 2 // 2 peticiones cada 10 segundos
                    }
                };
            });
        }
    }
}