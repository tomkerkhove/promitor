﻿using System;
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
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceMetricValidator();
                case ResourceType.VirtualMachine:
                    return new VirtualMachineMetricValidator();
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceMetricValidator();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryMetricValidator();
                case ResourceType.CosmosDb:
                    return new CosmosDbMetricValidator();
                case ResourceType.RedisCache:
                    return new RedisCacheMetricValidator();
                case ResourceType.PostgreSql:
                    return new PostgreSqlMetricValidator();
                case ResourceType.AzureSqlDatabase:
                    return new AzureSqlDatabaseMetricValidator();
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), $"No validation rules are defined for metric type '{resourceType}'");
        }
    }
}
