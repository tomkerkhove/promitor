﻿using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryMetricDefinition : MetricDefinition
    {
        public ContainerRegistryMetricDefinition()
        {
        }

        public ContainerRegistryMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string registryName, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            RegistryName = registryName;
        }

        public string RegistryName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerRegistry;
    }
}