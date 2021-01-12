using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TechSupportApp.Application;
using TechSupportApp.Application.Interfaces;
using TechSupportApp.Infrastructure.Identity;
using TechSupportApp.Infrastructure.Persistence;

namespace TechSupportApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));

            services.AddScoped<IAppContext>(provider => provider.GetService<ApplicationContext>());
            services.AddTransient<IUserService, DevIdentityUserService>();

           
        }

        [Obsolete("Use actual database")]
        private static void CreateInMemoryContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
        }
    }
}
