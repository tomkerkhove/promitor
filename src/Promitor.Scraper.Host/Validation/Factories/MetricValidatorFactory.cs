using System;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;
using Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes;

namespace Promitor.Scraper.Host.Validation.Factories
{
    internal static class MetricValidatorFactory
    {
        internal static IMetricValidator GetValidatorFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Generic:
                    return new GenericAzureMetricValidator();
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueMetricValidator();
                case ResourceType.StorageQueue:
                    return new StorageQueueMetricValidator();
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), $"No validation rules are defined for metric type '{resourceType}'");
        }
    }
}
