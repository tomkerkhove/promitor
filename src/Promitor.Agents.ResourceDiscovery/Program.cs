using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = CreateConfiguration(args);
            IHostBuilder webHostBuilder = CreateHostBuilder(args, configuration);

            return webHostBuilder;
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddYamlFile("/config/resource-declaration.yaml", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables("PROMITOR_")
                    .Build();

            return configuration;
        }

        private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            string httpEndpointUrl = "http://+:" + configuration["ARCUS_HTTP_PORT"];
            IHostBuilder webHostBuilder =
                Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configBuilder => configBuilder.AddConfiguration(configuration))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureKestrel(kestrelServerOptions => kestrelServerOptions.AddServerHeader = false)
                                  .UseUrls(httpEndpointUrl)
                                  .ConfigureLogging(logging => logging.AddConsole())
                                  .UseStartup<Startup>();
                    });

            return webHostBuilder;
        }
    }
}
