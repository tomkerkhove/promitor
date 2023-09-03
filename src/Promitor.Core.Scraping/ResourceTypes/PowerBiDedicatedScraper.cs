using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
	public class PowerBiDedicatedScraper :AzureMonitorScraper<PowerBiDedicatedResourceDefinition>
	{
        	private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.PowerBIDedicated/capacities/{2}";

        	public PowerBiDedicatedScraper(ScraperConfiguration scraperConfiguration)
            		: base(scraperConfiguration)
        	{
        	}

       		protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, PowerBiDedicatedResourceDefinition resource)
        	{
            		return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.CapacityName);
        	}
    	}
}
