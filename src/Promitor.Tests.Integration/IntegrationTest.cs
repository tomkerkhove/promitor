using Arcus.Testing.Logging;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class IntegrationTest
    {
        protected IConfiguration Configuration { get; }
        protected XunitTestLogger Logger { get; }

        public IntegrationTest(ITestOutputHelper testOutput)
        {
            Logger = new XunitTestLogger(testOutput);

            // The appsettings.local.json allows users to override (gitignored) settings locally for testing purposes
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json")
                .AddJsonFile(path: "appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
