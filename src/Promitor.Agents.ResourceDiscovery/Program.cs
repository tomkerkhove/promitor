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
        public static int Main(string[] args)
        {
            try
            {
                // TODO: ASCII Art with Welcome

                // Let's hook in a logger for start-up purposes.
                ConfigureStartupLogging();

                CreateHostBuilder(args)
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = BuildConfiguration(args);
            ServerConfiguration serverConfiguration = GetServerConfiguration(configuration);
            IHostBuilder webHostBuilder = CreatePromitorWebHost<Startup>(args, configuration, serverConfiguration);

            return webHostBuilder;
        }

        private static ServerConfiguration GetServerConfiguration(IConfiguration configuration)
        {
            var serverConfiguration = configuration.GetSection("server").Get<ServerConfiguration>();
            return serverConfiguration;
        }

        private static IConfiguration BuildConfiguration(string[] args)
        {
            var configurationFolder = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Folder);
            if (string.IsNullOrWhiteSpace(configurationFolder))
            {
                throw new Exception("Unable to determine the configuration folder");
            }

            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)
                    .AddYamlFile($"{configurationFolder}/runtime.yaml", optional: false, reloadOnChange: true)
                    .AddYamlFile($"{configurationFolder}/resource-discovery-declaration.yaml", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables("PROMITOR_")
                    .Build();

            return configuration;
        }
    }
}
