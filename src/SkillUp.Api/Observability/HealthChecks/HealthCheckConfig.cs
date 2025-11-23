using Microsoft.Extensions.DependencyInjection;


namespace SkillUp.Api.Observability.HealthChecks;


public static class HealthCheckConfig
{
    public static void AddHealthChecksConfig(this IServiceCollection services)
    {
        services.AddHealthChecks();
        // add DB, Redis, etc checks here
    }
}