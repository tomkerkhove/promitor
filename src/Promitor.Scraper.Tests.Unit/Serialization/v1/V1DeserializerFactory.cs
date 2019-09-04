using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1
{
    /// <summary>
    /// Used to create a deserializer for use by tests.
    /// </summary>
    public static class V1DeserializerFactory
    {
        public static V1Deserializer CreateDeserializer()
        {
            var logger = NullLogger.Instance;
            return new V1Deserializer(
                new AzureMetadataDeserializer(logger),
                new MetricDefaultsDeserializer(
                    new AggregationDeserializer(logger),
                    new ScrapingDeserializer(logger),
                    logger),
                new MetricDefinitionDeserializer(
                    new AzureMetricConfigurationDeserializer(
                        new MetricAggregationDeserializer(logger),
                        logger),
                    new ScrapingDeserializer(logger),
                    new AzureResourceDeserializerFactory(new SecretDeserializer(logger), logger),
                    logger),
                logger);
        }
    }
}
