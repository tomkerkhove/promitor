using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

// ReSharper disable All

namespace Promitor.Core.Scraping
{
    /// <summary>
    ///     Azure Monitor Scraper
    /// </summary>
    /// <typeparam name="TResourceDefinition">Type of metric definition that is being used</typeparam>
    public abstract class AppServiceScraper<TResourceDefinition> : AzureMonitorScraper<TResourceDefinition>
      where TResourceDefinition : class, IAppServiceResourceDefinition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected AppServiceScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
        }

        /// <summary>
        /// Builds the URI without deployment slots of the App Service resource to scrape
        /// </summary>
        /// <param name="subscriptionId">Subscription id in which the resource lives</param>
        /// <param name="scrapeDefinition">Contains all the information needed to scrape the resource.</param>
        /// <param name="resource">Contains the resource cast to the specific resource type.</param>
        /// <returns>Uri of Azure resource</returns>
        protected abstract string BuildResourceUriWithoutDeploymentSlot(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resource);

        /// <inheritdoc />
        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, TResourceDefinition resource)
        {
            var slotName = DetermineSlotName(resource);
            var resourceUri = BuildResourceUriWithoutDeploymentSlot(subscriptionId, scrapeDefinition, resource);

            // Production slot should not be suffixed in resource URI
            if (slotName != "production")
            {
                resourceUri += $"/slots/{slotName}";
            }

            return resourceUri;
        }

        /// <inheritdoc />
        protected override Dictionary<string, string> DetermineMetricLabels(TResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            var slotName = DetermineSlotName(resourceDefinition);
            metricLabels?.TryAdd("slot_name", slotName);

            return metricLabels;
        }

        private static string DetermineSlotName(TResourceDefinition resource)
        {
            return string.IsNullOrWhiteSpace(resource.SlotName) ? "production" : resource.SlotName;
        }
    }
}