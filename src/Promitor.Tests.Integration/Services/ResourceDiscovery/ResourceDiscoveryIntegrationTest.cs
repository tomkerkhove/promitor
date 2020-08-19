using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.ResourceDiscovery
{
    [Trait("Agent", "Resource Discovery")]
    public class ResourceDiscoveryIntegrationTest: IntegrationTest
    {
        public ResourceDiscoveryIntegrationTest(ITestOutputHelper testOutput) : base(testOutput)
        {
        }
    }
}
