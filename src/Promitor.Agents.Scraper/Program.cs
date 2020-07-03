using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Scraper.Validation;
using Promitor.Core;
using Serilog;

namespace Promitor.Agents.Scraper
{
    public class Program : AgentProgram
    {
        private const string RuntimeConfigFilename = "runtime.yaml";

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

                var host = CreateHostBuilder(args, configurationFolder)
                    .Build();
                
                using (var scope = host.Services.CreateScope())
                {
                    var validator = scope.ServiceProvider.GetRequiredService<RuntimeValidator>();
                    if (!validator.Validate())
                    {
                        Log.Logger.Fatal("Promitor is not configured correctly. Please fix validation issues and re-run.");
                        return (int)ExitStatus.ValidationFailed;
                    }

                    Log.Logger.Information("Promitor configuration is valid, we are good to go.");                    
                }

                host.Run();

                return (int)ExitStatus.Success;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Promitor Scraper Agent has encountered an unexpected error. Please open an issue at https://github.com/tomkerkhove/promitor/issues to let us know about it.");
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
            IConfiguration configuration = BuildConfiguration(configurationFolder);
            ServerConfiguration serverConfiguration = GetServerConfiguration(configuration);
            IHostBuilder webHostBuilder = CreatePromitorWebHost<Startup>(args, configuration, serverConfiguration);

            return webHostBuilder;
        }

        private static ServerConfiguration GetServerConfiguration(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();
            return serverConfiguration;
        }

        private static IConfigurationRoot BuildConfiguration(string configurationFolder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile($"{configurationFolder}/runtime.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "PROMITOR_") // Used for all environment variables for Promitor
                .AddEnvironmentVariables(prefix: "PROMITOR_YAML_OVERRIDE_") // Used to overwrite runtime YAML
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

            return null;
        }
    }
}