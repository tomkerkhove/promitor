﻿namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    public abstract class MetricDefinition
    {
        /// <summary>
        ///     Configuration about the Azure Monitor metric to scrape
        /// </summary>
        public AzureMetricConfiguration AzureMetricConfiguration { get; set; }

        /// <summary>
        ///     Description concerning metric that will be made available in the scraping endpoint
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Name of the metric to use when exposing in the scraping endpoint
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of resource that is configured
        /// </summary>
        public abstract ResourceType ResourceType { get; }
    }
}
