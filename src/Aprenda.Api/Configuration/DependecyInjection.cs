using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aprenda.Application.Interfaces;
using Aprenda.Application.Implementations;

namespace Aprenda.Api.Configuration
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITrailService, TrailService>();

            services.Configure<JwtConfig>(configuration.GetSection("Jwt"));
        }
    }
}
