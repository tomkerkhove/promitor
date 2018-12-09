using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Core;

namespace Promitor.Scraper.Host
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            var httpPort = DetermineHttpPort();
            var endpointUrl = $"http://+:{httpPort}";

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(kestrelServerOptions =>
                {
                    kestrelServerOptions.AddServerHeader = false;
                })
                .UseUrls(endpointUrl)
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .Welcome()
                .ValidateSetup()
                .Run();
        }

        private static int DetermineHttpPort()
        {
            var rawConfiguredHttpPort = Environment.GetEnvironmentVariable(EnvironmentVariables.Runtime.HttpPort);
            if (int.TryParse(rawConfiguredHttpPort, out int configuredHttpPort))
            {
                return configuredHttpPort;
            }

            return 80;
        }
    }
}