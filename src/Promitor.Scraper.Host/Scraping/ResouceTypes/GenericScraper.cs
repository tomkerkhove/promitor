using System.Threading.Tasks;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Scraping.ResouceTypes
{
    internal class GenericScraper : Scraper<GenericMetricDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/{2}";

        public GenericScraper(AzureMetadata azureMetadata, AzureCredentials azureCredentials, IExceptionTracker exceptionTracker)
            : base(azureMetadata, azureCredentials, exceptionTracker)
        {
        }

        protected override async Task<double> ScrapeResourceAsync(AzureMonitorClient azureMonitorClient, GenericMetricDefinition metricDefinition)
        {
            var resourceUri = string.Format(ResourceUriTemplate, AzureMetadata.SubscriptionId, AzureMetadata.ResourceGroupName, metricDefinition.ResourceUri);
            var metricName = metricDefinition.AzureMetricConfiguration.MetricName;
            var foundMetricValue = await azureMonitorClient.QueryMetricAsync(metricName, metricDefinition.AzureMetricConfiguration.Aggregation, resourceUri, metricDefinition.Filter);

            return foundMetricValue;
        }
    }
}