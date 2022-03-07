using System;
using System.Runtime.InteropServices;
using GuardNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core.Configuration.Server;
using Serilog;
using Version = Promitor.Core.Version;

namespace Promitor.Agents.Core
{
    public class AgentProgram
    {
        /// <summary>
        ///     Builds a web host according to Promitor standards
        /// </summary>
        /// <typeparam name="TStartup">type of startup class</typeparam>
        /// <param name="args">Startup arguments</param>
        /// <param name="configuration">General agent configuration</param>
        /// <param name="serverConfiguration">Configuration with regards to the web server</param>
        public static IHostBuilder CreatePromitorWebHost<TStartup>(string[] args, IConfiguration configuration, ServerConfiguration serverConfiguration) where TStartup : class
        {
            var httpPort = DetermineHttpPort(serverConfiguration);
            var httpEndpointUrl = $"http://+:{httpPort}";

            IHostBuilder webHostBuilder =
                Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configBuilder => configBuilder.AddConfiguration(configuration))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
                            .UseUrls(httpEndpointUrl)
                            .UseStartup<TStartup>();
                    })
                    .UseSerilog();

            return webHostBuilder;
        }

        /// <summary>
        ///     Determines HTTP port to expose agent on
        /// </summary>
        /// <param name="serverConfiguration">Configuration for server</param>
        protected static int DetermineHttpPort(ServerConfiguration serverConfiguration)
        {
            Guard.NotNull(serverConfiguration, nameof(serverConfiguration));

            return serverConfiguration?.HttpPort ?? 80;
        }

        /// <summary>
        ///     Configure logging during startup
        /// </summary>
        protected static void ConfigureStartupLogging()
        {
            string agentVersion = Version.Get();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Version", agentVersion)
                .WriteTo.Console()
                .CreateLogger();
        }

        /// <summary>
        ///     Welcome our users and boot up!
        /// </summary>
        protected static void Welcome()
        {
            string agentVersion = Version.Get();
            Console.WriteLine(Constants.Texts.Welcome);
            Log.Logger.Information($"Booting up Promitor v{agentVersion} running .NET {RuntimeInformation.FrameworkDescription} - Thank you for using Promitor!");
            var operatingSystem = RuntimeInformation.OSDescription.Contains("linux", StringComparison.InvariantCultureIgnoreCase) ? "Linux" : "Windows";
            Log.Logger.Information($"Running {RuntimeInformation.FrameworkDescription} on {operatingSystem} ({RuntimeInformation.RuntimeIdentifier} | {RuntimeInformation.OSDescription}).");
        }
    }
}