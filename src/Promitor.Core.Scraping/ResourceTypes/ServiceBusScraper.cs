using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public abstract class ServiceBusScraper<TMetricDefinition> : Scraper<TMetricDefinition> where TMetricDefinition : MetricDefinition, new()
    {
        protected const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.ServiceBus/namespaces/{2}";

        protected ServiceBusScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected string GetResourceUri(string subscriptionId, string resourceGroupName, string serviceBusNamespace)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, resourceGroupName, serviceBusNamespace);
        }
    }
}