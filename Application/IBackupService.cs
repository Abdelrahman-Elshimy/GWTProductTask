using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IBackupService
    {
        IFormFile GetBackUpFile();
        void SendEmail(string mailTo, string body, string subject, IList<IFormFile> attachements = null);
    }
}
