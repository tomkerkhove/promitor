namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinition : MetricDefinition
    {
        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.NetworkInterface;
    }
}