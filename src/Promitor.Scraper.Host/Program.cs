using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Model.Server;
using Serilog;

namespace Promitor.Scraper.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Welcome();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            BuildWebHost(args)
                .Build()
                .Run();
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public static IHostBuilder BuildWebHost(string[] args)
        {
            var configuration = CreateConfiguration();
            var httpPort = DetermineHttpPort(configuration);
            var endpointUrl = $"http://+:{httpPort}";

            // TODO: Configure verbosity https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.0#add-providers

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseKestrel(kestrelServerOptions => { kestrelServerOptions.AddServerHeader = false; })
                        .UseConfiguration(configuration)
                        .UseUrls(endpointUrl)
                        .UseStartup<Startup>();
                });
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("/config/runtime.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "PROMITOR_") // Used for all environment variables for Promitor
                .AddEnvironmentVariables(prefix: "PROMITOR_YAML_OVERRIDE_") // Used to overwrite runtime YAML
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