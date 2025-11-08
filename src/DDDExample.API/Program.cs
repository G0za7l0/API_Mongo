using DDDExample.API;
using DDDExample.API.Middleware;
using DDDExample.Infrastructure.Persistence.MongoDB;
using Microsoft.OpenApi.Models;

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

// Configuración MongoDB para ResponseTimeLogs (opcional si tu repositorio lo requiere)
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

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

// Middleware de métricas de tiempo de respuesta
app.UseResponseTimeLogging();

app.UseAuthorization();

app.MapControllers(); // Mapea todos los controladores

app.Run();
