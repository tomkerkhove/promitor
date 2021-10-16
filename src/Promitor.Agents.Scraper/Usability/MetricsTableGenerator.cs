using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Humanizer;
using Promitor.Agents.Core.Usability;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Spectre.Console;

namespace Promitor.Agents.Scraper.Usability
{
    public class MetricsTableGenerator : AsciiTableGenerator
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;

        public MetricsTableGenerator(IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));

            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        ///     Plots all configured metric information into an ASCII table
        /// </summary>
        public void PlotOverviewInAsciiTable()
        {
            var metricsDeclaration = _metricsDeclarationProvider.Get();
            PlotAzureMetadataInAsciiTable(metricsDeclaration.AzureMetadata);
            PlotMetricsInAsciiTable(metricsDeclaration.Metrics);
        }

        private void PlotAzureMetadataInAsciiTable(AzureMetadata metadata)
        {
            var asciiTable = CreateAzureMetadataAsciiTable();

            asciiTable.AddRow(metadata.TenantId, metadata.SubscriptionId, metadata.ResourceGroupName, metadata.Cloud.Name.Humanize(LetterCasing.Title));

            AnsiConsole.Write(asciiTable);
        }

        private void PlotMetricsInAsciiTable(List<MetricDefinition> configuredMetrics)
        {
            var asciiTable = CreateMetricsAsciiTable();

            foreach (var metric in configuredMetrics)
            {
                string configuredResourceDiscoveryGroups = "None";
                if(metric.ResourceDiscoveryGroups?.Any() == true)
                {
                    configuredResourceDiscoveryGroups= string.Join(", ", metric.ResourceDiscoveryGroups.Select(grp => grp.Name));
                }

                string configuredResources = "None";
                if (metric.Resources?.Any() == true)
                {
                    configuredResources = string.Join(", ", metric.Resources.Select(grp => grp.ResourceName));
                }

                string labels = "None";
                if (metric.PrometheusMetricDefinition?.Labels?.Any() == true)
                {
                    labels = string.Empty;

                    foreach (var label in metric.PrometheusMetricDefinition.Labels)
                    {
                        labels += $"- {label.Key}: {label.Value}{Environment.NewLine}";
                    }
                }

                var outputMetricName = metric.PrometheusMetricDefinition?.Name ?? string.Empty;
                var azureMetricName = metric.AzureMetricConfiguration?.MetricName ?? string.Empty;

                asciiTable.AddRow(outputMetricName, metric.ResourceType.Humanize(LetterCasing.Title), labels, azureMetricName, configuredResources, configuredResourceDiscoveryGroups);
            }

            AnsiConsole.Write(asciiTable);
        }

        private Table CreateAzureMetadataAsciiTable()
        {
            var asciiTable = CreateAsciiTable("Azure Metadata");

            // Add some columns
            asciiTable.AddColumn("Tenant Id");
            asciiTable.AddColumn("Subscription Id");
            asciiTable.AddColumn("Resource Group Name (Default)");
            asciiTable.AddColumn("Azure Cloud");

            return asciiTable;
        }

        private Table CreateMetricsAsciiTable()
        {
            var asciiTable = CreateAsciiTable("Configured Metrics");

            // Add some columns
            asciiTable.AddColumn("Metric Name");
            asciiTable.AddColumn("Resource Type");
            asciiTable.AddColumn("Labels");
            asciiTable.AddColumn("Azure Monitor Metric");
            asciiTable.AddColumn("Resources To Scrape");
            asciiTable.AddColumn("Resource Discovery Groups To Scrape");

            return asciiTable;
        }
    }
}
