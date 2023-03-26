using Hangfire;
using Infrastructure.Context;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BakupJob
    {

        public static async void CreateBackupJob(IBackupService backupService)
        {
            string to = "abdoelsimy123@gmail.com";
            string body = "test body";
            string subject = "test subject";
            IList<IFormFile> files = new List<IFormFile>();
            IFormFile formFile = backupService.GetBackUpFile();
            if(formFile != null )
            {
                files.Add(formFile);
            }

            RecurringJob.AddOrUpdate<IBackupService>(job => backupService.SendEmail(to, body, subject, files), Cron.Minutely);


        }
    }
}
