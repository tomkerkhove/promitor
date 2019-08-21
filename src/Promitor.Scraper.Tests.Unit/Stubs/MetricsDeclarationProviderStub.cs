using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Providers;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricsDeclarationProviderStub : MetricsDeclarationProvider
    {
        private readonly string _rawMetricsDeclaration;

        public MetricsDeclarationProviderStub(string rawMetricsDeclaration, IMapper mapper) : base(configuration: null, logger: NullLogger.Instance, mapper: mapper, v2Deserializer: CreateDeserializer())
        {
            _rawMetricsDeclaration = rawMetricsDeclaration;
        }

        private static IDeserializer<MetricsDeclarationV2> CreateDeserializer()
        {
            var logger = new Mock<ILogger>();

            return new V2Deserializer(
                new AzureMetadataDeserializer(logger.Object),
                new MetricDefaultsDeserializer(
                    new AggregationDeserializer(logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    logger.Object),
                new MetricDefinitionDeserializer(
                    new AzureMetricConfigurationDeserializer(
                        new MetricAggregationDeserializer(logger.Object),
                        logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    new AzureResourceDeserializerFactory(new SecretDeserializer(logger.Object), logger.Object),
                    logger.Object),
                logger.Object);
        }

        public override string ReadRawDeclaration()
        {
            return _rawMetricsDeclaration;
        }
    }
}