using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sistema_bancario.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la cadena de conexi�n a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SisContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Agregar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Bancario API V1");
        c.RoutePrefix = string.Empty; // Swagger estar� en la ra�z
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

// Abrir el navegador autom�ticamente cuando la aplicaci�n inicie
app.Lifetime.ApplicationStarted.Register(() =>
{
    var url = app.Urls.FirstOrDefault(u => u.StartsWith("http"));
    if (!string.IsNullOrEmpty(url))
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            Console.WriteLine("No se pudo abrir el navegador autom�ticamente.");
        }
    }
});

app.Run();
