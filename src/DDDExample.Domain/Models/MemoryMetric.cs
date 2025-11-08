using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDDExample.Domain.Models;

public class MemoryMetric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public double ProcessMemoryMB { get; set; }
    public double GCMemoryMB { get; set; }
    public double CpuUsage { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
