using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.Application.Interfaces;
using SkillUp.Application.Implementations;

namespace SkillUp.Api.Configuration
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
