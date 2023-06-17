using Arcus.Testing.Logging;
using Microsoft.Extensions.Configuration;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class IntegrationTest
    {
        protected IConfiguration Configuration { get; }
        protected XunitTestLogger Logger { get; }
        public PrometheusClientFactory PrometheusClientFactory => new(Logger);

        public IntegrationTest(ITestOutputHelper testOutput)
        {
            Logger = new XunitTestLogger(testOutput);

            // The appsettings.local.json allows users to override (gitignored) settings locally for testing purposes
            Configuration = ConfigurationFactory.Create();
        }
    }
}
