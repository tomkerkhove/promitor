using Promitor.Scraper.Host.Configuration.Providers;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider, IMetricsDeclarationProvider
    {
        private readonly string rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration)
        {
            this.rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string GetSerializedDeclaration()
        {
            return rawMetricsDeclaration;
        }
    }
}