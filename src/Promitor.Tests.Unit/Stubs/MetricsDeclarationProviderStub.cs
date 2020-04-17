using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Tests.Unit.Serialization.v1;

namespace Promitor.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration, IMapper mapper) : base(null, new ConfigurationSerializer(NullLogger<ConfigurationSerializer>.Instance, mapper, V1DeserializerFactory.CreateDeserializer()))
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}