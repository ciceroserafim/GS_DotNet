using Microsoft.Extensions.DependencyInjection;
using SkillUp.Application.Interfaces;
using SkillUp.Application.Implementations;

namespace SkillUp.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITrailService, TrailService>();

            return services;
        }
    }
}
