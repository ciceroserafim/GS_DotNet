using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aprenda.Api.Middlewares;
using Aprenda.Application.DependencyInjection;
using Aprenda.Infrastructure.DependencyInjection;
using Aprenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Aprenda.Api.Configuration;

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
// OpenTelemetry (Tracing e Metrics)
// -----------------------------
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: "Aprenda+.API", serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource("Aprenda.API")
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddConsoleExporter());

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
        Title = "Aprenda+ API",
        Version = "v1",
        Description = "API para gestão do Aprenda+ - O Futuro do Trabalho"
    });

    // Configuração JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// -----------------------------
// JWT Authentication
// -----------------------------
var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>() ?? new JwtConfig();
var key = Encoding.UTF8.GetBytes(jwtConfig.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// -----------------------------
// Dependency Injection
// -----------------------------
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aprenda+ API v1");
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
