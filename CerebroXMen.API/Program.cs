using CerebroXMen.Infrastructure.Data;
using CerebroXMen.Infrastructure.Repositories;
using CerebroXMen.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CerebroXMen.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("RAILWAY_DB_URL")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<CerebroDbContext>(options =>
    options.UseNpgsql(connectionString));

// Inyección de dependencias
builder.Services.AddScoped<IDnaRepository, DnaRepository>();
builder.Services.AddScoped<IMutantDetectorService, MutantDetectorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();