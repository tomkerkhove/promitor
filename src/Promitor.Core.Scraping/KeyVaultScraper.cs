﻿using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

// ReSharper disable All

namespace Promitor.Core.Scraping
{
    /// <summary>
    /// Scrapes an Azure Key Vault
    /// </summary>
    public class KeyVaultScraper : AzureMonitorScraper<KeyVaultResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.KeyVault/vaults/{2}";

        public KeyVaultScraper(ScraperConfiguration scraperConfiguration) :
            base(scraperConfiguration)
        {
        }

        /// <inheritdoc />
        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, KeyVaultResourceDefinition resource)
        {
            return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.VaultName);
        }
    }
}