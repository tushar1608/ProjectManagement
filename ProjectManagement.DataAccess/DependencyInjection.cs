using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.DataAccess.Interfaces;
using ProjectManagement.DataAccess.Services;

namespace ProjectManagement.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection DataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ProjectManagementDb"));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
