using System;
using System.Collections.Generic;
using GuardNet;
using Humanizer;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Usability;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Spectre.Console;

namespace Promitor.Agents.ResourceDiscovery.Usability
{
    public class DiscoveryGroupTableGenerator : AsciiTableGenerator
    {
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        public DiscoveryGroupTableGenerator(IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor)
        {
            Guard.NotNull(resourceDeclarationMonitor, nameof(resourceDeclarationMonitor));

            _resourceDeclarationMonitor = resourceDeclarationMonitor;
        }

        /// <summary>
        ///     Plots all configured information into an ASCII table
        /// </summary>
        public void PlotOverviewInAsciiTable()
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            PlotAzureMetadataInAsciiTable(resourceDeclaration.AzureLandscape);
            PlotResourceDiscoveryGroupsInAsciiTable(resourceDeclaration.ResourceDiscoveryGroups);
        }

        private void PlotResourceDiscoveryGroupsInAsciiTable(List<ResourceDiscoveryGroup> resourceDiscoveryGroups)
        {
            var asciiTable = CreateResourceDiscoveryGroupsAsciiTable();

            foreach (var resourceDiscoveryGroup in resourceDiscoveryGroups)
            {
                var isInclusionCriteriaConfigured = resourceDiscoveryGroup.Criteria.Include != null ? "Yes" : "No";
                asciiTable.AddRow(resourceDiscoveryGroup.Name, resourceDiscoveryGroup.Type.Humanize(LetterCasing.Title), isInclusionCriteriaConfigured);
            }

            AnsiConsole.Write(asciiTable);
        }

        private void PlotAzureMetadataInAsciiTable(AzureLandscape azureLandscape)
        {
            var asciiTable = CreateAzureMetadataAsciiTable();

            var rawSubscriptions = "- " + string.Join($"{Environment.NewLine} - ", azureLandscape.Subscriptions);

            asciiTable.AddRow(azureLandscape.TenantId, azureLandscape.Cloud.Humanize(LetterCasing.Title), rawSubscriptions);

            AnsiConsole.Write(asciiTable);
        }

        private Table CreateResourceDiscoveryGroupsAsciiTable()
        {
            var asciiTable = CreateAsciiTable("Resource Discovery Groups");

            // Add some columns
            asciiTable.AddColumn("Name");
            asciiTable.AddColumn("Resource Type");
            asciiTable.AddColumn("Is Include Criteria Configured?");

            return asciiTable;
        }

        private Table CreateAzureMetadataAsciiTable()
        {
            var asciiTable = CreateAsciiTable("Azure Landscape");

            // Add some columns
            asciiTable.AddColumn("Tenant Id");
            asciiTable.AddColumn("Azure Cloud");
            asciiTable.AddColumn("Subscriptions");

            return asciiTable;
        }
    }
}
