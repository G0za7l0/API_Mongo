using MongoDB.Driver;
using DDDExample.Domain.Entities;

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

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

        // Puedes agregar otras colecciones aquí
        // public IMongoCollection<ResponseTimeLog> ResponseTimeLogs => _database.GetCollection<ResponseTimeLog>("responseTimeLogs");
    }
}
