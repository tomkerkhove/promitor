using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinitionV1 : MetricDefinitionV1
    {
        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.NetworkInterface;

        public override MetricDefinition Build()
        {
            return new NetworkInterfaceMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                NetworkInterfaceName,
                Labels,
                Scraping.Build());
        }
    }
}