using System.Collections.Generic;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    /// <summary>
    ///     Scrapes an Azure Automation account.
    /// </summary>
    public class AutomationAccountScraper : AzureMonitorScraper<AutomationAccountResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Automation/automationAccounts/{2}";

        public AutomationAccountScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, AutomationAccountResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.AccountName);
        }

        protected override string DetermineMetricFilter(string metricName, AutomationAccountResourceDefinition resourceDefinition)
        {
            if (string.IsNullOrWhiteSpace(resourceDefinition.RunbookName))
            {
                return base.DetermineMetricFilter(metricName, resourceDefinition);
            }

            return $"Runbook eq '{resourceDefinition.RunbookName}'";
        }

        protected override Dictionary<string, string> DetermineMetricLabels(AutomationAccountResourceDefinition resourceDefinition)
        {
            var labels = base.DetermineMetricLabels(resourceDefinition);

            if (string.IsNullOrWhiteSpace(resourceDefinition.RunbookName) == false)
            {
                labels.Add("runbook_name", resourceDefinition.RunbookName);
            }

            return labels;
        }
    }
}