using AutoMapper;
using DDDExample.Application.Interfaces;
using DDDExample.Application.Mappings;
using DDDExample.Application.Services;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Persistence.MongoDB;
using DDDExample.Infrastructure.Repositories;
using DDDExample.API.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDExample.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration config)
        {
            // Configurar MongoDBSettings desde appsettings.json
            var mongoSettings = config.GetSection("MongoDBSettings").Get<MongoDbSettings>();
            if (mongoSettings == null || string.IsNullOrEmpty(mongoSettings.ConnectionString))
            {
                throw new ArgumentNullException(nameof(mongoSettings), "MongoDB settings are not configured in appsettings.json");
            }

            // Registrar MongoDbContext pasando el objeto MongoDbSettings
            services.AddSingleton(new MongoDbContext(mongoSettings));

            // Repositorios
            services.AddScoped<IProductRepository, ProductRepository>();

            // Servicios de aplicación
            services.AddScoped<IProductService, ProductService>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Middleware de Response Time Logging
            services.AddResponseTimeLogging();

            return services;
        }
    }
}
