using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillUp.Api.Middlewares;
using SkillUp.Application.DependencyInjection;
using SkillUp.Infrastructure.DependencyInjection;
using SkillUp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// -----------------------------
// Logging com Serilog
// -----------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// -----------------------------
// Controllers e Health Checks
// -----------------------------
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// -----------------------------
// DbContext com SQL Server
// -----------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -----------------------------
// Swagger
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SkillUp API",
        Version = "v1",
        Description = "API para gestão do SkillUp"
    });
});

// -----------------------------
// Dependency Injection
// -----------------------------
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication();

// -----------------------------
// Versionamento global da API
// -----------------------------
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// -----------------------------
// Construção da aplicação
// -----------------------------
var app = builder.Build();

// -----------------------------
// Middlewares
// -----------------------------
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// -----------------------------
// Swagger Middleware
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkillUp API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz: http://localhost:<porta>/
    });
}

// -----------------------------
// Map Controllers e HealthCheck
// -----------------------------
app.MapControllers();
app.MapHealthChecks("/health");

// -----------------------------
// Executar a aplicação
// -----------------------------
app.Run();
