using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Promitor.Core.Contracts;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public abstract class AzureMessagingScraper<TResourceDefinition> : AzureMonitorScraper<TResourceDefinition> where TResourceDefinition : class, IAzureResourceDefinition
    {
        private const string EntityNameLabel = "entity_name";

        protected AzureMessagingScraper(ScraperConfiguration scraperConfiguration)
            : base(scraperConfiguration)
        {
        }

        protected override List<MeasuredMetric> EnrichMeasuredMetrics(TResourceDefinition resourceDefinition, List<string> dimensionNames, List<MeasuredMetric> metricValues)
        {
            // Change Azure Monitor Dimension name to more representable value
            foreach (var measuredMetric in metricValues.Where(metricValue => metricValue.Dimensions.Any()))
            {
                measuredMetric.Dimensions[0].Name = EntityNameLabel;
            }

            return metricValues;
        }

        protected override Dictionary<string, string> DetermineMetricLabels(TResourceDefinition resourceDefinition)
        {
            var metricLabels = base.DetermineMetricLabels(resourceDefinition);

            if (IsEntityDeclared(resourceDefinition))
            {
                var entityName = GetEntityName(resourceDefinition);
                metricLabels.Add(EntityNameLabel, entityName);
            }

            return metricLabels;
        }

        protected override List<string> DetermineMetricDimensions(string metricName, TResourceDefinition resourceDefinition, AzureMetricConfiguration configuration)
        {
            if (IsEntityDeclared(resourceDefinition))
            {
                return base.DetermineMetricDimensions(metricName, resourceDefinition, configuration);
            }

            Logger.LogTrace("Using 'EntityName' dimension since no topic was configured.");

            return new List<string> { "EntityName" };
        }

        protected override string DetermineMetricFilter(string metricName, TResourceDefinition resourceDefinition)
        {
            var entityName = "*";

            if (IsEntityDeclared(resourceDefinition))
            {
                entityName = GetEntityName(resourceDefinition);
            }

            return $"EntityName eq '{entityName}'";
        }

        /// <summary>
        ///     Determines if an entity is declared or only the namespace
        /// </summary>
        /// <param name="resourceDefinition">Definition concerning the Azure resource</param>
        protected abstract bool IsEntityDeclared(TResourceDefinition resourceDefinition);

        /// <summary>
        ///     Determines the name of the entity in the namespace namespace
        /// </summary>
        /// <param name="resourceDefinition">Definition concerning the Azure resource</param>
        protected abstract string GetEntityName(TResourceDefinition resourceDefinition);
    }
}