using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Providers;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        // TODO: Pass in actual deserializer
        public MetricsDeclarationProviderStub(string rawMetricsDeclaration, IMapper mapper) : base(configuration: null, logger: NullLogger.Instance, mapper: mapper, v2Deserializer: null)
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}