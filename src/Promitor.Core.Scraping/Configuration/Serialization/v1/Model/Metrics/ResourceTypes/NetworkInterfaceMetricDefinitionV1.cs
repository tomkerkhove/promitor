using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinitionV1 : MetricDefinitionV1
    {
        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.NetworkInterface;
    }
}