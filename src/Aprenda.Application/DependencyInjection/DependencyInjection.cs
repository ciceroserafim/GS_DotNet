using Microsoft.Extensions.DependencyInjection;
using Aprenda.Application.Interfaces;
using Aprenda.Application.Implementations;

namespace Aprenda.Application.DependencyInjection
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
