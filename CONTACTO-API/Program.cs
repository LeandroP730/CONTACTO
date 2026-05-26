using CONTACTO_API.Middleware;
using CONTACTO_CORE.Repositories;
using CONTACTO_CORE.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IContactoRepository, InMemoryContactoRepository>();

builder.Services.AddScoped<IContactoService, ContactoService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }