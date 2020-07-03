using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Core;
using Serilog;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Program : AgentProgram
    {
        private const string RuntimeConfigFilename = "runtime.yaml";
        private const string ResourceDiscoveryFilename = "resource-discovery-declaration.yaml";

        public static int Main(string[] args)
        {
            try
            {
                Welcome();

                // Let's hook in a logger for start-up purposes.
                ConfigureStartupLogging();

                var configurationFolder = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Folder);
                var configurationExitStatus = ValidateConfigurationExists(configurationFolder);
                if (configurationExitStatus != null)
                {
                    return (int)configurationExitStatus;
                }

                CreateHostBuilder(args, configurationFolder)
                    .Build()
                    .Run();

                return (int)ExitStatus.Success;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Promitor Discovery Agent has encountered an unexpected error. Please open an issue at https://github.com/tomkerkhove/promitor/issues to let us know about it.");
                return (int)ExitStatus.UnhandledException;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void Welcome()
        {
            Console.WriteLine(Constants.Texts.Welcome);
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string configurationFolder)
        {
            IConfiguration configuration = BuildConfiguration(args, configurationFolder);
            ServerConfiguration serverConfiguration = GetServerConfiguration(configuration);
            IHostBuilder webHostBuilder = CreatePromitorWebHost<Startup>(args, configuration, serverConfiguration);

            return webHostBuilder;
        }

        private static ServerConfiguration GetServerConfiguration(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();
            return serverConfiguration;
        }

        private static IConfiguration BuildConfiguration(string[] args, string configurationFolder)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)
                    .AddYamlFile($"{configurationFolder}/{RuntimeConfigFilename}", optional: false, reloadOnChange: true)
                    .AddYamlFile($"{configurationFolder}/{ResourceDiscoveryFilename}", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables("PROMITOR_")
                    .Build();

            return configuration;
        }

        private static ExitStatus? ValidateConfigurationExists(string configurationFolder)
        {
            if (string.IsNullOrWhiteSpace(configurationFolder))
            {
                Log.Logger.Fatal($"Unable to determine the configuration folder. Please ensure that the '{EnvironmentVariables.Configuration.Folder}' environment variable is set");
                return ExitStatus.ConfigurationFolderNotSpecified;
            }

            var runtimeConfigPath = Path.Combine(configurationFolder, RuntimeConfigFilename);
            if (!File.Exists(runtimeConfigPath))
            {
                Log.Logger.Fatal($"Unable to find runtime configuration at '{runtimeConfigPath}'");
                return ExitStatus.ConfigurationFileNotFound;
            }

            var resourceDiscoveryConfigPath = Path.Combine(configurationFolder, ResourceDiscoveryFilename);
            if (!File.Exists(resourceDiscoveryConfigPath))
            {
                Log.Logger.Fatal($"Unable to find resource discovery configuration at '{resourceDiscoveryConfigPath}'");
                return ExitStatus.ConfigurationFileNotFound;
            }

            return null;
        }
    }
}
