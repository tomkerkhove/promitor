using System.Collections.Generic;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Integrations.AzureStorage;

namespace Promitor.Scraper.Tests.Unit.Builders
{
    public class MetricsDeclarationBuilder
    {
        private readonly AzureMetadata _azureMetadata;
        private readonly List<Core.Scraping.Configuration.Model.Metrics.MetricDefinition> _metrics = new List<Core.Scraping.Configuration.Model.Metrics.MetricDefinition>();

        public MetricsDeclarationBuilder(AzureMetadata azureMetadata)
        {
            _azureMetadata = azureMetadata;
        }

        public static MetricsDeclarationBuilder WithMetadata(string tenantId = "tenantId", string subscriptionId = "subscriptionId", string resourceGroupName = "resourceGroupName")
        {
            var azureMetadata = new AzureMetadata
            {
                TenantId = tenantId,
                SubscriptionId = subscriptionId,
                ResourceGroupName = resourceGroupName
            };

            return new MetricsDeclarationBuilder(azureMetadata);
        }

        public static MetricsDeclarationBuilder WithoutMetadata()
        {
            return new MetricsDeclarationBuilder(azureMetadata: null);
        }

        public string Build()
        {
            var metricsDeclaration = new MetricsDeclaration
            {
                AzureMetadata = _azureMetadata,
                Metrics = _metrics
            };

            var configurationSerializer = new ConfigurationSerializer(NullLogger.Instance);
            return configurationSerializer.Serialize(metricsDeclaration);
        }

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "promitor-service-bus", string metricDescription = "Description for a metric", string queueName = "promitor-queue", string serviceBusNamespace = "promitor-namespace", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var metric = new ServiceBusQueueMetricDefinition
            {
                ResourceType = ResourceType.ServiceBusQueue,
                Name = metricName,
                Description = metricDescription,
                QueueName = queueName,
                Namespace = serviceBusNamespace,
                AzureMetricConfiguration = azureMetricConfiguration
            };
            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithContainerInstanceMetric(string metricName = "promitor-container-instance", string metricDescription = "Description for a metric", string containerGroup = "promitor-group",  string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var metric = new ContainerInstanceMetricDefinition
            {
                ResourceType = ResourceType.ContainerInstance,
                Name = metricName,
                Description = metricDescription,
                ContainerGroup = containerGroup,
                AzureMetricConfiguration = azureMetricConfiguration
            };

            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithAzureStorageQueueMetric(string metricName = "promitor-storage-queue", string metricDescription = "Description for a metric", string queueName = "promitor-queue", string accountName = "promitor-account", string sasToken="?sig=promitor", string azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount)
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var metric = new StorageQueueMetricDefinition
            {
                ResourceType = ResourceType.StorageQueue,
                Name = metricName,
                Description = metricDescription,
                QueueName = queueName,
                AccountName = accountName,
                SasToken = sasToken,
                AzureMetricConfiguration = azureMetricConfiguration
            };
            _metrics.Add(metric);

            return this;
        }

        public MetricsDeclarationBuilder WithGenericMetric(string metricName = "foo", string metricDescription = "Description for a metric", string resourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging", string filter = "EntityName eq \'orders\'", string azureMetricName = "Total")
        {
            var azureMetricConfiguration = CreateAzureMetricConfiguration(azureMetricName);
            var metric = new GenericMetricDefinition
            {
                Name = metricName,
                Description = metricDescription,
                ResourceUri = resourceUri,
                Filter = filter,
                AzureMetricConfiguration = azureMetricConfiguration
            };
            _metrics.Add(metric);

            return this;
        }

        private AzureMetricConfiguration CreateAzureMetricConfiguration(string azureMetricName)
        {
            return new AzureMetricConfiguration
            {
                MetricName = azureMetricName,
                Aggregation = new MetricAggregation
                {
                    Type = AggregationType.Average
                }
            };
        }
    }
}