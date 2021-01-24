using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    public class AutomationAccountMetricValidator: IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<AutomationAccountResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.AccountName))
                {
                    yield return "No automation account name is configured";
                }
            }
        }
    }
}
