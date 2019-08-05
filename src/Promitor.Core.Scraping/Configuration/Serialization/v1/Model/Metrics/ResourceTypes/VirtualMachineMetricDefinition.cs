namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class VirtualMachineMetricDefinition : MetricDefinition
    {
        public string VirtualMachineName { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.VirtualMachine;
    }
}