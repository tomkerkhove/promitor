using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Scraper.Tests.Unit.Serialization.v1;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration, IMapper mapper) : base(configuration: null, logger: NullLogger.Instance, mapper: mapper, v1Deserializer: V1DeserializerFactory.CreateDeserializer())
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}