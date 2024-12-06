using DevFreela.API.ExceptionHandler;
using DevFreela.Application;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configura��es - appSettings.json
//builder.Services.Configure<FreelanceTotalCostConfig>(
//    builder.Configuration.GetSection("FreelanceTotalCostConfig")
//    );

#region Inje��o de Dependencia
// Singleton
//builder.Services.AddSingleton<IConfigService, ConfigService>();
// Foi alterado para injetar a depend�ncia direto nas camadas
#endregion

//builder.Services.AddDbContext<DevFreelaDbContext>(o => o.UseInMemoryDatabase("DevFreelaDb"));
var connectionString = builder.Configuration.GetConnectionString("DevFreelaCS");
builder.Services.AddDbContext<DevFreelaDbContext>(o => o.UseSqlServer(connectionString));

builder.Services.AddApplication();

#region Exception Handler
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

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
