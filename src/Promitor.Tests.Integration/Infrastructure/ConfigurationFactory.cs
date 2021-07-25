using Microsoft.Extensions.Configuration;

namespace Promitor.Tests.Integration.Infrastructure
{
    public class ConfigurationFactory
    {
        public static IConfiguration Create()
        {
            // The appsettings.local.json allows users to override (gitignored) settings locally for testing purposes
            return new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json")
                .AddJsonFile(path: "appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
