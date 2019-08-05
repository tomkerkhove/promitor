namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinition : MetricDefinition
    {
        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.NetworkInterface;
    }
}