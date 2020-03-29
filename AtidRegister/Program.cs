using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AtidRegister
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHostBuilder = CreateHostBuilder(args).Build(); // create and build webhost
            webHostBuilder.Run(); // run application
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        // Build existing, default configuration
                        var settings = config.Build();
                        // Add Azure for sensitive data (not that sensitive)
                        config.AddAzureAppConfiguration(settings["ConnectionStrings:AppConfig"]);
                    })
                    .UseStartup<Startup>(); // use startup class
                });
    }
}
