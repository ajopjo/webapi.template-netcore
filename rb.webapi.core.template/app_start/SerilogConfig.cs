using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Webapi.Core.Template.Configurations;

namespace Webapi.Core.Template
{
    public static class SerilogConfig
    {
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureLogging((hostingcontext, logging) =>
            {
                var logSettings = new LogSettings();
                hostingcontext.Configuration.GetSection(nameof(LogSettings)).Bind(logSettings);

                if (string.IsNullOrEmpty(logSettings.ApplicationName))
                {
                    throw new ArgumentException("ApplicationName is missing from LogSettings");
                }

                if (logSettings.Sink.Equals("rollingFile"))
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.WithProperty("ApplicationName", logSettings.ApplicationName)
                        .Enrich.FromLogContext()
                        .WriteTo.RollingFile(logSettings.Url)
                        .CreateLogger();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.WithProperty("ApplicationName", logSettings.ApplicationName)
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Seq(logSettings.Url)
                        .CreateLogger();
                }

                var levelSwitch = new LoggingLevelSwitch();
                int.TryParse(logSettings.LogLevel, out var level);
                levelSwitch.MinimumLevel = (LogEventLevel)level;
                logging.AddSerilog(dispose: true);
            });
            return webHostBuilder;
        }
    }

}
 
