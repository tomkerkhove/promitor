namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineMetricDefinition : MetricDefinition
    {
        public string VirtualMachineName { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.VirtualMachine;
    }
}