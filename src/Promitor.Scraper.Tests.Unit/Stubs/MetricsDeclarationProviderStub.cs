using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider, IMetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration) : base(NullLogger.Instance)
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}