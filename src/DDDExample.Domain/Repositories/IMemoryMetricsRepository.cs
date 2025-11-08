using DDDExample.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDExample.Domain.Repositories;

public interface IMemoryMetricsRepository
{
    Task AddAsync(MemoryMetric metric);
    Task<IEnumerable<MemoryMetric>> GetRecentMetricsAsync(int limit = 100);
}
