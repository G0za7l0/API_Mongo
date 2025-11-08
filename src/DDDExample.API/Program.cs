using DDDExample.API;
using DDDExample.API.Middleware;
using DDDExample.Infrastructure.Persistence.MongoDB;
using DDDExample.Application.Services;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Persistence.MongoDB.Repositories;
using DDDExample.API.HealthChecks;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Configuración de servicios
// -------------------------------
builder.Services.AddControllers();                // Controladores API
builder.Services.AddEndpointsApiExplorer();       // Endpoints para Swagger

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DDDExample API",
        Version = "v1",
        Description = "API para el proyecto DDDExample con MongoDB"
    });
});

// Inyección de dependencias personalizada (MongoDB, Repositorios, Servicios, AutoMapper, Middleware)
builder.Services.AddProjectServices(builder.Configuration);

// -------------------------------
// Configuración MongoDB para métricas de memoria
// -------------------------------
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Repositorio y servicio de métricas de memoria
builder.Services.AddSingleton<IMemoryMetricsRepository, MemoryMetricsRepository>();
builder.Services.Configure<MemoryMetricsSettings>(
    builder.Configuration.GetSection("MemoryMetrics"));
builder.Services.AddHostedService<MemoryMetricsService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<MemoryHealthCheck>("memory_check");

// -------------------------------
// Construcción de la app
// -------------------------------
var app = builder.Build();

// -------------------------------
// Configuración del pipeline HTTP
// -------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDExample API v1");
    });
}

app.UseHttpsRedirection();

// Middleware de métricas de tiempo de respuesta (opcional)
app.UseResponseTimeLogging();

app.UseAuthorization();

app.MapControllers(); // Mapea todos los controladores

// Endpoints de Health Checks
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/memory", new HealthCheckOptions
{
    Predicate = (check) => check.Name == "memory_check"
});

app.Run();
