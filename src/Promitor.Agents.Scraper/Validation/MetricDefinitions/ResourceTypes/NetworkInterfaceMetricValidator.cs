using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class NetworkInterfaceMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<NetworkInterfaceResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.NetworkInterfaceName))
                {
                    errorMessages.Add("No network interface name is configured");
                }
            }

            return errorMessages;
        }
    }
}