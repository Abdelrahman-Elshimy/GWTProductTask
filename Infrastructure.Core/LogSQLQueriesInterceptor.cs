using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data.Common;
using System.IO;

namespace Infrastructure.Core
{

    public class LogSQLQueriesInterceptor : DbCommandInterceptor
    {
        private readonly IConfiguration _configuration;
        private const bool defaultEnable = false;
        private const int defaultThreshold = 500;

        private bool Enable
        {
            get
            {
                bool enable = defaultEnable;
                if (_configuration["AppSettings:SQLLog_Enable"] != null)
                {
                    if (!bool.TryParse(_configuration["AppSettings:SQLLog_Enable"], out enable))
                        enable = defaultEnable;
                }
                return enable;
            }
        }

        private string Location
        {
            get
            {
                string location = null;
                if (_configuration["AppSettings:SQLLog_Location"] != null)
                {
                    location = _configuration["AppSettings:SQLLog_Location"];
                }
                return location;
            }
        }

        private int Threshold
        {
            get
            {
                int threshold = defaultThreshold;
                if (_configuration["AppSettings:SQLLog_Threshold"] != null)
                {
                    if (!int.TryParse(_configuration["AppSettings:SQLLog_Threshold"], out threshold))
                        threshold = defaultThreshold;
                }
                return threshold;
            }
        }

        public LogSQLQueriesInterceptor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            if (Enable)
            {
                string location = Location;
                if (!string.IsNullOrWhiteSpace(location))
                {
                    DateTime now = DateTime.UtcNow;

                    if (!Directory.Exists(location))
                        Directory.CreateDirectory(location);

                    location = Path.Combine(location, now.Year.ToString());

                    if (!Directory.Exists(location))
                        Directory.CreateDirectory(location);

                    location = Path.Combine(location, now.Month.ToString());

                    if (!Directory.Exists(location))
                        Directory.CreateDirectory(location);

                    location = Path.Combine(location, now.Day.ToString());

                    if (!Directory.Exists(location))
                        Directory.CreateDirectory(location);

                    if (eventData.Duration.TotalMilliseconds > Threshold)
                    {
                        string path = Path.Combine(location, Guid.NewGuid() + ".txt");

                        var json = JsonConvert.SerializeObject(new
                        {
                            Date = DateTime.UtcNow,
                            Query = command.CommandText,
                            Duration = eventData.Duration.TotalMilliseconds,
                        }, Formatting.Indented);

                        File.AppendAllText(path, json);
                    }
                }
            }
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
