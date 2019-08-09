using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class CosmosDbMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Cosmos DB metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Cosmos DB configuration</param>
        /// <returns>A new <see cref="MetricDefinitionV1"/> object (strongly typed as a <see cref="CosmosDbMetricDefinitionV1"/>) </returns>
        internal override MetricDefinitionV1 Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<CosmosDbMetricDefinitionV1>(metricNode);

            var dbName = metricNode.Children[new YamlScalarNode("dbName")];

            metricDefinition.DbName = dbName?.ToString();

            return metricDefinition;
        }
    }
}
