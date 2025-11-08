using System.Diagnostics;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DDDExample.API.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseTimeMiddleware> _logger;
        private readonly IResponseTimeLogRepository _repository;
        private readonly long _thresholdMs;

        public ResponseTimeMiddleware(
            RequestDelegate next,
            ILogger<ResponseTimeMiddleware> logger,
            IResponseTimeLogRepository repository,
            long thresholdMs = 500)
        {
            _next = next;
            _logger = logger;
            _repository = repository;
            _thresholdMs = thresholdMs;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/favicon.ico"))
            {
                await _next(context);
                return;
            }

            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();

            var duration = watch.ElapsedMilliseconds;
            var isSlow = duration > _thresholdMs;

            var log = new ResponseTimeLog
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.Value,
                DurationMs = duration,
                StatusCode = context.Response.StatusCode,
                ClientIp = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"],
                IsSlowRequest = isSlow
            };

            await _repository.AddAsync(log);

            if (isSlow)
                _logger.LogWarning($"Slow request: {context.Request.Method} {context.Request.Path} took {duration}ms");
            else
                _logger.LogInformation($"{context.Request.Method} {context.Request.Path} took {duration}ms");
        }
    }
}
