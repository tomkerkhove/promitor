using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class VirtualMachineMetricDefinitionV1 : MetricDefinitionV1
    {
        public string VirtualMachineName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.VirtualMachine;

        public override MetricDefinition Build()
        {
            return new VirtualMachineMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                VirtualMachineName,
                Labels,
                Scraping.Build());
        }
    }
}