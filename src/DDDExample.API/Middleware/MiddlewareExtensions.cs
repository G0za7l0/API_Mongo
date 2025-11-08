using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Repositories.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDDExample.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddResponseTimeLogging(this IServiceCollection services)
        {
            services.AddSingleton<IResponseTimeLogRepository, MongoResponseTimeLogRepository>();
            return services;
        }

        public static IApplicationBuilder UseResponseTimeLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
