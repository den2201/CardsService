using CardService.AppConfiguration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

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
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseIIS();
                    webBuilder.ConfigureKestrel(kestrel =>
                    {
                        kestrel.Limits.MaxConcurrentConnections = 5;
                        kestrel.Limits.MaxRequestHeaderCount = 7;
                        kestrel.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(5);
                    });
                });
    }
}
