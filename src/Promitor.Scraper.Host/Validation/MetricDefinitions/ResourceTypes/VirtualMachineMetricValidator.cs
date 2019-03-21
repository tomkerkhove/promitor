using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    public class VirtualMachineMetricValidator : IMetricValidator<VirtualMachineMetricDefinition>
    {
        public List<string> Validate(VirtualMachineMetricDefinition virtualMachineMetricDefinition)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(virtualMachineMetricDefinition.VirtualMachineName))
            {
                errorMessages.Add("No virtual machine name is configured");
            }

            return errorMessages;
        }
    }
}