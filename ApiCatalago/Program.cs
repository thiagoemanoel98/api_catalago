using System.Text.Json.Serialization;
using ApiCatalago.Context;
using ApiCatalago.Extensions;
using ApiCatalago.Filters;
using ApiCatalago.Logging;
using ApiCatalago.Repositories;
using ApiCatalago.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

// Configurando conexão com o Banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
// ReferenceHandler.IgnoreCycles: Evita erros de ciclo de objeto
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExeptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
    
builder.Services.AddOpenApi();



// Quando invoca IMyService, resolve MyService
builder.Services.AddTransient<IMyService, MyService>();
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/openapi/v1.json", "Minha API V1");
    });
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();