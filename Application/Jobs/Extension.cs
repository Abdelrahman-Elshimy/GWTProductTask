using Application;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Application
{
    public static class Extension
    {
       
        public static IHost CreateJob(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                IBackupService backupService = services.GetRequiredService<IBackupService>();
                BakupJob.CreateBackupJob(backupService);
            }
            return host;
        }
    }
}
