using System;
using System.IO;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Core;
using Serilog;
using Serilog.Events;

namespace Promitor.Agents.Core
{
    public class AgentProgram
    {
        /// <summary>
        ///     Determines HTTP port to expose agent on
        /// </summary>
        /// <param name="serverConfiguration">Configuration for server</param>
        protected int DetermineHttpPort(ServerConfiguration serverConfiguration)
        {
            Guard.NotNull(serverConfiguration, nameof(serverConfiguration));

            return serverConfiguration?.HttpPort ?? 80;
        }

        /// <summary>
        ///     Configure logging during startup
        /// </summary>
        protected void ConfigureStartupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        /// <summary>
        ///     Builds configuration for application
        /// </summary>
        protected IConfigurationRoot BuildConfiguration()
        {
            var configurationFolder = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Folder);
            if (string.IsNullOrWhiteSpace(configurationFolder))
                throw new Exception("Unable to determine the configuration folder");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile($"{configurationFolder}/runtime.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "PROMITOR_") // Used for all environment variables for Promitor
                .AddEnvironmentVariables(prefix: "PROMITOR_YAML_OVERRIDE_") // Used to overwrite runtime YAML
                .Build();

            return configuration;
        }
    }
}