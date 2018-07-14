using System.Collections.Generic;
using Microsoft.Azure.Management.Monitor.Models;
using Promitor.Scraper.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;
using Promitor.Scraper.Serialization;

namespace Promitor.Scraper.Tests.Unit.Builders
{
    public class MetricsDeclarationBuilder
    {
        private readonly AzureMetadata azureMetadata;
        private readonly List<ServiceBusQueueMetricDefinition> metrics = new List<ServiceBusQueueMetricDefinition>();

        public MetricsDeclarationBuilder(AzureMetadata azureMetadata)
        {
            this.azureMetadata = azureMetadata;
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
                AzureMetadata = azureMetadata,
                Metrics = metrics
            };

            var serializer = YamlSerialization.CreateSerializer();
            return serializer.Serialize(metricsDeclaration);
        }

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "foo", string metricDescription = "Description for a metric", string queueName = "foo-queue", string serviceBusNamespace = "foo-space", string azureMetricName = "Total")
        {
            var metric = new ServiceBusQueueMetricDefinition
            {
                ResourceType = ResourceType.ServiceBusQueue,
                Name = metricName,
                Description = metricDescription,
                QueueName = queueName,
                Namespace = serviceBusNamespace,
                AzureMetricConfiguration = new AzureMetricConfiguration
                {
                    MetricName = azureMetricName,
                    Aggregation = AggregationType.Average
                }
            };
            metrics.Add(metric);

            return this;
        }
    }
}