using DDDExample.Domain.Models;
using DDDExample.Domain.Repositories;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Persistence.MongoDB.Repositories;

public class MemoryMetricsRepository : IMemoryMetricsRepository
{
    private readonly IMongoCollection<MemoryMetric> _collection;

    public MemoryMetricsRepository(MongoDbContext context)
    {
        _collection = context.Database.GetCollection<MemoryMetric>("MemoryMetrics");
    }

    public async Task AddAsync(MemoryMetric metric) =>
        await _collection.InsertOneAsync(metric);

    public async Task<IEnumerable<MemoryMetric>> GetRecentMetricsAsync(int limit = 100) =>
        await _collection.Find(_ => true)
                         .SortByDescending(m => m.Timestamp)
                         .Limit(limit)
                         .ToListAsync();
}
