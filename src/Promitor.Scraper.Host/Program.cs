using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Promitor.Core;
using Promitor.Scraper.Host.Extensions;

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
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.Sources.Clear();
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddYamlFile("/config/runtime.yaml", optional: false, reloadOnChange: true);
                })
                .UseUrls(endpointUrl)
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args)
        {
            Welcome();

            BuildWebHost(args)
                .ValidateSetup()
                .Run();
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
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