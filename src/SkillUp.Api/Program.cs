using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillUp.Api.Middlewares;
using SkillUp.Api.Observability.Logging;
using SkillUp.Application.DependencyInjection;
using SkillUp.Infrastructure.DependencyInjection;
using SkillUp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Logging com Serilog
// -----------------------------
builder.Host.UseSerilogLogging(builder.Configuration);

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
// Map Controllers e HealthCheck
// -----------------------------
app.MapControllers();
app.MapHealthChecks("/health");

// -----------------------------
// Log de URLs ao iniciar
// -----------------------------
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("🚀 API rodando nas seguintes URLs:");
    foreach (var url in app.Urls)
    {
        Console.WriteLine($"   {url}");
    }
});

// -----------------------------
// Executar a aplicação
// -----------------------------
app.Run();
