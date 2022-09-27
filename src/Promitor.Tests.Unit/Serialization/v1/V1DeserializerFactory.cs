using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;

namespace Promitor.Tests.Unit.Serialization.v1
{
    /// <summary>
    /// Used to create a deserializer for use by tests.
    /// </summary>
    public static class V1DeserializerFactory
    {
        public static V1Deserializer CreateDeserializer()
        {
            return new V1Deserializer(
                new AzureMetadataDeserializer(NullLogger<AzureMetadataDeserializer>.Instance),
                new MetricDefaultsDeserializer(
                    new AggregationDeserializer(NullLogger<AggregationDeserializer>.Instance),
                    new ScrapingDeserializer(NullLogger<ScrapingDeserializer>.Instance),
                    NullLogger<MetricDefaultsDeserializer>.Instance),
                new MetricDefinitionDeserializer(
                    new AzureMetricConfigurationDeserializer(
                        new MetricDimensionDeserializer(NullLogger<MetricDimensionDeserializer>.Instance), 
                        new MetricAggregationDeserializer(NullLogger<MetricAggregationDeserializer>.Instance),
                        NullLogger<AzureMetricConfigurationDeserializer>.Instance),
                    new LogAnalyticsConfigurationDeserializer(new AggregationDeserializer(NullLogger<AggregationDeserializer>.Instance), NullLogger<LogAnalyticsConfigurationDeserializer>.Instance),
                    new ScrapingDeserializer(NullLogger<ScrapingDeserializer>.Instance),
                    new AzureResourceDiscoveryGroupDeserializer(NullLogger<AzureResourceDiscoveryGroupDeserializer>.Instance),
                    new AzureResourceDeserializerFactory(new SecretDeserializer(NullLogger<SecretDeserializer>.Instance), new LoggerFactory()),
                    NullLogger<MetricDefinitionDeserializer>.Instance),
                NullLogger<V1Deserializer>.Instance);
        }
    }
}
