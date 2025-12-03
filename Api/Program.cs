using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ZONA DE SERVICIOS (Configuración antes de crear la app) ---

// Agregar controladores (necesario para tu API)
builder.Services.AddControllers();

// Agregar Swagger/OpenAPI (útil para probar la API desde el navegador)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar SQLite (Conexión a Base de Datos)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS (Permitir que el Frontend Blazor hable con este Backend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
        policy.AllowAnyOrigin()   // En producción se pone la URL específica, para dev está bien así
              .AllowAnyMethod()   // GET, POST, PUT, DELETE
              .AllowAnyHeader()); // Headers como Content-Type
});

// --- Construir la aplicación ---
var app = builder.Build();

// --- 2. ZONA DE MIDDLEWARE (Cómo responde la app a las peticiones) ---

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANTE: UseCors debe ir ANTES de UseAuthorization y MapControllers
app.UseCors("AllowBlazor");

app.UseAuthorization();

// Mapear los controladores (conectar las rutas URL a tu código C#)
app.MapControllers();

// Ejecutar la aplicación
app.Run();