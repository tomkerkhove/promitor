using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    internal class StorageQueueMetricDeserializer : MetricDeserializer
    {
        /// <summary>Deserializes the specified Storage Queue metric node from the YAML configuration file.</summary>
        /// <param name="metricNode">The metric node to deserialize to Storage queue configuration</param>
        /// <returns>A new <see cref="MetricDefinitionV1"/> object (strongly typed as a <see cref="StorageQueueMetricDefinitionV1"/>) </returns>
        internal override MetricDefinitionV1 Deserialize(YamlMappingNode metricNode)
        {
            var metricDefinition = base.DeserializeMetricDefinition<StorageQueueMetricDefinitionV1>(metricNode);
            var accountName = metricNode.Children[new YamlScalarNode("accountName")];
            var queueName = metricNode.Children[new YamlScalarNode("queueName")];
            
            metricDefinition.AccountName = accountName?.ToString();
            metricDefinition.QueueName = queueName?.ToString();

            var secretDeserializer = new SecretDeserializer(Logger);

            if (metricNode.Children.ContainsKey("sasToken"))
            {
                var sasTokenNode = (YamlMappingNode)metricNode.Children["sasToken"];
                metricDefinition.SasToken = secretDeserializer.Deserialize(sasTokenNode);
            }
            else
            {
                Logger.LogError($"No SAS token was configured for Azure Storage Account '{accountName}'");
            }

            return metricDefinition;
        }
    }
}
