using DevFreela.API.ExceptionHandler;
using DevFreela.Application;
using DevFreela.Infrastructure;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurações - appSettings.json
//builder.Services.Configure<FreelanceTotalCostConfig>(
//    builder.Configuration.GetSection("FreelanceTotalCostConfig")
//    );

#region Injeção de Dependencia
// Singleton
//builder.Services.AddSingleton<IConfigService, ConfigService>();
// Foi alterado para injetar a dependência direto nas camadas
#endregion

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

#region Exception Handler
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Exception Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
