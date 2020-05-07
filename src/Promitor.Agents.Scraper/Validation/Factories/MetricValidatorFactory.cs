using System;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.Factories
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
                case ResourceType.SqlDatabase:
                    return new SqlDatabaseMetricValidator();
                case ResourceType.SqlManagedInstance:
                    return new SqlManagedInstanceMetricValidator();
                case ResourceType.VirtualMachineScaleSet:
                    return new VirtualMachineScaleSetMetricValidator();
                case ResourceType.WebApp:
                    return new WebAppMetricValidator();
                case ResourceType.AppPlan:
                    return new AppPlanMetricValidator();
                case ResourceType.FunctionApp:
                    return new FunctionAppMetricValidator();
                case ResourceType.ApiManagement:
                    return new ApiManagementMetricValidator();
                case ResourceType.StorageAccount:
                    return new StorageAccountMetricValidator();
                case ResourceType.BlobStorage:
                    return new BlobStorageMetricValidator();
                case ResourceType.FileStorage:
                    return new FileStorageMetricValidator();
                case ResourceType.SqlServer:
                    return new SqlServerMetricValidator();
                case ResourceType.IoTHub:
                    return new IoTHubMetricValidator();
                case ResourceType.DeviceProvisioningService:
                    return new DeviceProvisioningServiceMetricValidator();
                case ResourceType.KeyVault:
                    return new KeyVaultMetricValidator();
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), $"No validation rules are defined for metric type '{resourceType}'");
        }
    }
}
