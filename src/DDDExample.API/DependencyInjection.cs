using AutoMapper;
using DDDExample.Application.Interfaces;
using DDDExample.Application.Mappings;
using DDDExample.Application.Services;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Persistence;
using DDDExample.Infrastructure.Repositories;

namespace DDDExample.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration config)
        {
            var mongoSection = config.GetSection("MongoDB");
            var connectionString = mongoSection["ConnectionString"];
            var databaseName = mongoSection["DatabaseName"];

            services.AddSingleton(new MongoDbContext(connectionString, databaseName));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
