using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.DataAccess;

namespace ProjectManagement.Web.iTest
{
    public class WebApplicationFactory
    {
        public class WebAppFactory<T> : WebApplicationFactory<Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (dbContext != null)
                        services.Remove(dbContext);

                    var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemory");
                        options.UseInternalServiceProvider(serviceProvider);
                    });
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                        {
                            try
                            {
                                appContext.Database.EnsureCreated();
                            }
                            catch (Exception ex)
                            {
                                
                                throw;
                            }
                        }
                    }
                });
            }
        }
    }
}
