using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinitionBuilder : MetricDefinitionBuilder
    {
        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.NetworkInterface;

        public override MetricDefinition Build()
        {
            return new NetworkInterfaceMetricDefinition(
                AzureMetricConfigurationBuilder.Build(),
                Description,
                Name,
                ResourceGroupName,
                NetworkInterfaceName,
                Labels,
                ScrapingBuilder.Build());
        }
    }
}