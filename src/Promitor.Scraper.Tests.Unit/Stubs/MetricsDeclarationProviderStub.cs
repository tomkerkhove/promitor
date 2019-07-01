using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Providers;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration) : base(configuration: null, logger: NullLogger.Instance)
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}