using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class NetworkInterfaceMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            var errorMessages = new List<string>();

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<NetworkInterfaceMetricDefinition>())
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