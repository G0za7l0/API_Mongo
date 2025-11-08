using DDDExample.API;
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

// Inyección de dependencias personalizada (MongoDB, Repositorios, Servicios, AutoMapper)
builder.Services.AddProjectServices(builder.Configuration);

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
app.UseAuthorization();

app.MapControllers(); // Mapea todos los controladores

app.Run();
