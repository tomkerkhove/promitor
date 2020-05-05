﻿using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class IotHubMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<IotHubResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.IotHubName))
                {
                    yield return "No iot hub name is configured";
                }
            }
        }
    }
}