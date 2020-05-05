using GuardNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core.Configuration.Server;
using Serilog;

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
                            .UseSerilog()
                            .UseStartup<TStartup>();
                    });

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
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}