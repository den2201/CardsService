using CardService.AppConfiguration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CardService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", optional: false)
                   .Build();
                    AppSettings app = new AppSettings();
                    config.GetSection("AppSettings").Bind(app);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(kestrel =>
                    {
                        kestrel.Limits.MaxConcurrentConnections = 5;
                        kestrel.Limits.MaxRequestHeaderCount = 7;
                        kestrel.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(5);
                    });
                    webBuilder.UseUrls(app.AppUrlHttp, app.AppUrlHttps);
                });
    }
}
