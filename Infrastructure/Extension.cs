using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Infrastructure
{
    public static class Extension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                       Console.WriteLine(ex);
                    }
                }
            }
            return host;
        }

        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                IHostEnvironment hostEnvironment = services.GetRequiredService<IHostEnvironment>();

                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>())
                {
                    DataSeeder.Seed(context, hostEnvironment);
                }
            }
            return host;
        }
    }
}
