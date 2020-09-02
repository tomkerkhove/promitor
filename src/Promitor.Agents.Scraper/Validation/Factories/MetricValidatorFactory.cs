﻿using System;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Validation.Factories
{
    internal static class MetricValidatorFactory
    {
        internal static IMetricValidator GetValidatorFor(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ApiManagement:
                    return new ApiManagementMetricValidator();
                case ResourceType.ApplicationGateway:
                    return new ApplicationGatewayMetricValidator();
                case ResourceType.AppPlan:
                    return new AppPlanMetricValidator();
                case ResourceType.BlobStorage:
                    return new BlobStorageMetricValidator();
                case ResourceType.ContainerInstance:
                    return new ContainerInstanceMetricValidator();
                case ResourceType.ContainerRegistry:
                    return new ContainerRegistryMetricValidator();
                case ResourceType.CosmosDb:
                    return new CosmosDbMetricValidator();
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceMetricValidator();
                case ResourceType.EventHubs:
                    return new EventHubsMetricValidator();
                case ResourceType.ExpressRouteCircuit:
                    return new ExpressRouteCircuitMetricsValidator();
                case ResourceType.FileStorage:
                    return new FileStorageMetricValidator();
                case ResourceType.FunctionApp:
                    return new FunctionAppMetricValidator();
                case ResourceType.Generic:
                    return new GenericAzureMetricValidator();
                case ResourceType.IoTHub:
                    return new IoTHubMetricValidator();
                case ResourceType.KeyVault:
                    return new KeyVaultMetricValidator();
                case ResourceType.LogicApp:
                    return new LogicAppMetricValidator();
                case ResourceType.NetworkInterface:
                    return new NetworkInterfaceMetricValidator();
                case ResourceType.PostgreSql:
                    return new PostgreSqlMetricValidator();
                case ResourceType.RedisCache:
                    return new RedisCacheMetricValidator();
                case ResourceType.ServiceBusQueue:
                    return new ServiceBusQueueMetricValidator();
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseMetricValidator();
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceMetricValidator();
                case ResourceType.SqlServer:
                    return new SqlServerMetricValidator();
                case ResourceType.StorageAccount:
                    return new StorageAccountMetricValidator();
                case ResourceType.StorageQueue:
                    return new StorageQueueMetricValidator();
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetMetricValidator();
                case ResourceType.VirtualMachine:
                    return new VirtualMachineMetricValidator();
                case ResourceType.WebApp:
                    return new WebAppMetricValidator();
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), $"No validation rules are defined for metric type '{resourceType}'");
        }
    }
}
