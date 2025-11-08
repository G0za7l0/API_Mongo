using DDDExample.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DDDExample.API.Controllers;

[ApiController]
[Route("api/metrics/memory")]
public class MetricsController : ControllerBase
{
    private readonly IMemoryMetricsRepository _repository;

    public MetricsController(IMemoryMetricsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecent()
    {
        var metrics = await _repository.GetRecentMetricsAsync();
        return Ok(metrics);
    }
}
