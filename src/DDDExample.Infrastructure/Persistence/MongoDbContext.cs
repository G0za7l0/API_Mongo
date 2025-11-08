using MongoDB.Driver;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Models;

namespace DDDExample.Infrastructure.Persistence.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(MongoDbSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException(nameof(settings.ConnectionString));

            if (string.IsNullOrEmpty(settings.DatabaseName))
                throw new ArgumentNullException(nameof(settings.DatabaseName));

            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        // Propiedad pública para acceder a la base de datos directamente
        public IMongoDatabase Database => _database;

        // Colecciones de dominio
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<MemoryMetric> MemoryMetrics => _database.GetCollection<MemoryMetric>("MemoryMetrics");

        // Puedes agregar otras colecciones según tu dominio
        // public IMongoCollection<ResponseTimeLog> ResponseTimeLogs => _database.GetCollection<ResponseTimeLog>("responseTimeLogs");
    }
}
