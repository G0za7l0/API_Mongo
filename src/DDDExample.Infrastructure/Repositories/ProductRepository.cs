using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Persistence;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;
        public ProductRepository(MongoDbContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products.Find(_ => true).ToListAsync();

        public async Task<Product?> GetByIdAsync(string id) =>
            await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Product product) =>
            await _context.Products.InsertOneAsync(product);

        public async Task UpdateAsync(Product product) =>
            await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);

        public async Task DeleteAsync(string id) =>
            await _context.Products.DeleteOneAsync(p => p.Id == id);
    }
}
