using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class CosmosDbMetricDeserializer : GenericAzureMetricDeserializer
    {
        /// <summary>Deserializes the specified Cosmos DB metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Cosmos DB configuration</param>
        /// <returns>A new <see cref="MetricDefinition"/> object (strongly typed as a <see cref="CosmosDbMetricDefinition"/>) </returns>
        internal override MetricDefinition Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<CosmosDbMetricDefinition>(metricNode);

            var dbName = metricNode.Children[new YamlScalarNode("dbName")];

            metricDefinition.DbName = dbName?.ToString();

            return metricDefinition;
        }
    }
}
