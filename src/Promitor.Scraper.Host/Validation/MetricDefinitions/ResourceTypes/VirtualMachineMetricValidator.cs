using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class VirtualMachineMetricValidator : MetricValidator<VirtualMachineMetricDefinition>
    {
        protected override IEnumerable<string> Validate(VirtualMachineMetricDefinition virtualMachineMetricDefinition)
        {
            Guard.NotNull(virtualMachineMetricDefinition, nameof(virtualMachineMetricDefinition));

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(virtualMachineMetricDefinition.VirtualMachineName))
            {
                errorMessages.Add("No virtual machine name is configured");
            }

            return errorMessages;
        }
    }
}