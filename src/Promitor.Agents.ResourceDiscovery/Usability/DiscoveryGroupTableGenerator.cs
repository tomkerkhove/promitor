using System;
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
        }

        private void PlotAzureMetadataInAsciiTable(AzureLandscape azureLandscape)
        {
            var asciiTable = CreateAzureMetadataAsciiTable();

            var rawSubscriptions = "- " + string.Join($"{Environment.NewLine} - ", azureLandscape.Subscriptions);

            asciiTable.AddRow(azureLandscape.TenantId, azureLandscape.Cloud.Humanize(LetterCasing.Title), rawSubscriptions);

            AnsiConsole.Render(asciiTable);
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
