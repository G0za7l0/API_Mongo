using System.Diagnostics;
using DDDExample.Domain.Models;
using DDDExample.Domain.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DDDExample.Application.Services;

public class MemoryMetricsSettings
{
    public int CollectionIntervalSeconds { get; set; }
    public int WarningThresholdMB { get; set; }
    public int CriticalThresholdMB { get; set; }
}

public class MemoryMetricsService : BackgroundService
{
    private readonly ILogger<MemoryMetricsService> _logger;
    private readonly MemoryMetricsSettings _settings;
    private readonly IMemoryMetricsRepository _repository;
    private readonly PerformanceCounter _cpuCounter;

    public MemoryMetricsService(
        ILogger<MemoryMetricsService> logger,
        IOptions<MemoryMetricsSettings> settings,
        IMemoryMetricsRepository repository)
    {
        _logger = logger;
        _settings = settings.Value;
        _repository = repository;
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de métricas de memoria iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var memoryMB = process.WorkingSet64 / 1024.0 / 1024.0;
                var gcMemory = GC.GetTotalMemory(false) / 1024.0 / 1024.0;
                var cpuUsage = await GetCpuUsage();

                var status = memoryMB >= _settings.CriticalThresholdMB
                    ? "Critical"
                    : memoryMB >= _settings.WarningThresholdMB
                        ? "Warning"
                        : "Normal";

                await _repository.AddAsync(new MemoryMetric
                {
                    ProcessMemoryMB = memoryMB,
                    GCMemoryMB = gcMemory,
                    CpuUsage = cpuUsage,
                    Status = status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al recolectar métricas");
            }

            await Task.Delay(TimeSpan.FromSeconds(_settings.CollectionIntervalSeconds), stoppingToken);
        }
    }

    private async Task<float> GetCpuUsage()
    {
        _cpuCounter.NextValue();
        await Task.Delay(500);
        return _cpuCounter.NextValue();
    }
}
