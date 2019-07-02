using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Configuration.Model.Server;

namespace Promitor.Scraper.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Welcome();

            BuildWebHost(args)
                .Run();
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = CreateConfiguration();
            var t = Environment.GetEnvironmentVariables();
            var httpPort = DetermineHttpPort(configuration);
            var endpointUrl = $"http://+:{httpPort}";

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(kestrelServerOptions =>
                {
                    kestrelServerOptions.AddServerHeader = false;
                })
                .UseConfiguration(configuration)
                .UseUrls(endpointUrl)
                .UseStartup<Startup>()
                .Build();
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("/config/runtime.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "PROMITOR:")
                .Build();

            return configuration;
        }

        private static int DetermineHttpPort(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();

            return serverConfiguration?.HttpPort ?? 80;
        }
    }
}