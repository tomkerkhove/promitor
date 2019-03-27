using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class NetworkInterfaceMetricValidator : MetricValidator<NetworkInterfaceMetricDefinition>
    {
        protected override IEnumerable<string> Validate(NetworkInterfaceMetricDefinition networkInterfaceMetricDefinition)
        {
            Guard.NotNull(networkInterfaceMetricDefinition, nameof(networkInterfaceMetricDefinition));

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(networkInterfaceMetricDefinition.NetworkInterfaceName))
            {
                errorMessages.Add("No network interface name is configured");
            }

            return errorMessages;
        }
    }
}