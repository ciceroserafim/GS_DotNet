using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SkillUp.Api.Observability.Logging
{
    public static class SerilogConfig
    {
        public static void UseSerilogLogging(this IHostBuilder builder, IConfiguration config)
        {
            builder.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(config)
                    .Enrich.FromLogContext();
            });
        }
    }
}
