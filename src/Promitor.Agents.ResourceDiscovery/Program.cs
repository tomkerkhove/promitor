﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Extensions;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.ResourceDiscovery.Usability;
using Promitor.Core;
using Serilog;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Program : AgentProgram
    {
        public static int Main(string[] args)
        {
            try
            {
                // Let's hook in a logger for start-up purposes.
                ConfigureStartupLogging();

                Welcome();

                var configurationFolder = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Folder);
                if (string.IsNullOrWhiteSpace(configurationFolder))
                {
                    Log.Logger.Fatal($"Unable to determine the configuration folder. Please ensure that the '{EnvironmentVariables.Configuration.Folder}' environment variable is set");
                    return (int)ExitStatus.ConfigurationFolderNotSpecified;
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
                    PlotConfiguredDiscoveryGroups(scope);
                }

                host.Run();

                return (int)ExitStatus.Success;
            }
            catch (ConfigurationFileNotFoundException exception)
            {
                Log.Logger.Fatal($"Unable to find a required configuration file at '{exception.Path}'");
                return (int)ExitStatus.ConfigurationFileNotFound;
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
                    .AddRequiredYamlFile($"{configurationFolder}/runtime.yaml", reloadOnChange: true)
                    .AddRequiredYamlFile($"{configurationFolder}/resource-discovery-declaration.yaml", reloadOnChange: true)
                    .AddEnvironmentVariables("PROMITOR_")
                    .Build();

            return configuration;
        }

        private static void PlotConfiguredDiscoveryGroups(IServiceScope scope)
        {
            var metricsTableGenerator = scope.ServiceProvider.GetRequiredService<DiscoveryGroupTableGenerator>();
            Log.Logger.Information("Here's an overview of what was configured:");
            metricsTableGenerator.PlotOverviewInAsciiTable();
        }
    }
}
