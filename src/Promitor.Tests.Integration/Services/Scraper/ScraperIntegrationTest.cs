using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper
{
    [Trait("Agent", "Scraper")]
    public class ScraperIntegrationTest : IntegrationTest
    {
        public ScraperIntegrationTest(ITestOutputHelper testOutput) : base(testOutput)
        {
        }

        public string ExpectedVersion => Configuration["Agents:Scraper:Expectations:Version"];
    }
}
