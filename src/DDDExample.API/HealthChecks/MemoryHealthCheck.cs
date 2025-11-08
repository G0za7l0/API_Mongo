using Microsoft.Extensions.Diagnostics.HealthChecks;
using DDDExample.Application.Services;
using Microsoft.Extensions.Options;

namespace DDDExample.API.HealthChecks;

public class MemoryHealthCheck : IHealthCheck
{
    private readonly MemoryMetricsSettings _settings;

    public MemoryHealthCheck(IOptions<MemoryMetricsSettings> settings)
    {
        _settings = settings.Value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var processMemoryMB = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
        var status = processMemoryMB >= _settings.CriticalThresholdMB
            ? HealthStatus.Unhealthy
            : processMemoryMB >= _settings.WarningThresholdMB
                ? HealthStatus.Degraded
                : HealthStatus.Healthy;

        return Task.FromResult(
            status == HealthStatus.Healthy
                ? HealthCheckResult.Healthy($"Memory usage is normal: {processMemoryMB} MB")
                : status == HealthStatus.Degraded
                    ? HealthCheckResult.Degraded($"Memory usage warning: {processMemoryMB} MB")
                    : HealthCheckResult.Unhealthy($"Memory usage critical: {processMemoryMB} MB")
        );
    }
}
