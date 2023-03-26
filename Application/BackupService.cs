using Infrastructure.Core.Integrations.SendEmailSettings;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application
{
    public class BackupService : IBackupService
    {
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private readonly IOptionsMonitor<MailSettings> _mailSettings;

        [Obsolete]
        public BackupService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, IOptionsMonitor<MailSettings> mailSettings)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
            _mailSettings = mailSettings;
        }

        [Obsolete]
        public IFormFile GetBackUpFile()
        {
            string MyDatabase = "ProductTaskDB";
            string newBackUpName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "backup";
            var backupsPath = Path.Combine(hostingEnvironment.ContentRootPath, "Backups");


            SqlCommand sqlRestoreCommand = new SqlCommand($@"BACKUP DATABASE [{MyDatabase} ]TO  DISK = '{backupsPath}\{newBackUpName}.bak'");

            string conString = ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();
                    sqlRestoreCommand.Connection = connection;
                    sqlRestoreCommand.CommandTimeout = int.MaxValue;
                    sqlRestoreCommand.ExecuteNonQuery();
                    sqlRestoreCommand.Dispose();
                    string fileName = newBackUpName + ".bak";
                    string filePath = Path.Combine(backupsPath, fileName);

                    var bytes = File.ReadAllBytes(filePath);
                    var stream = new MemoryStream(bytes);

                    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(fileName));
                    return file;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public void SendEmail(string mailTo, string body, string subject, IList<IFormFile> attachements = null)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.CurrentValue.Email),
                    Subject = subject
                };
                var x = _mailSettings.CurrentValue.Email;
                email.To.Add(MailboxAddress.Parse(mailTo));
                var builder = new BodyBuilder();
                if (attachements != null)
                {
                    byte[] fileBytes;
                    foreach (var file in attachements)
                    {
                        if (file.Length > 0)
                        {
                            using var sm = new MemoryStream();
                            file.CopyTo(sm);
                            fileBytes = sm.ToArray();
                            builder.Attachments.Add(file.FileName, fileBytes);
                        }
                    }
                }
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                email.From.Add(new MailboxAddress(_mailSettings.CurrentValue.DisplayName, _mailSettings.CurrentValue.Email));
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587);
                smtp.Authenticate(_mailSettings.CurrentValue.Email, _mailSettings.CurrentValue.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }catch
            {

            }
        }

       
    }
}