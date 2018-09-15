﻿using System.Collections.Generic;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Configuration.Serialization;

namespace Promitor.Scraper.Tests.Unit.Builders
{
    public class MetricsDeclarationBuilder
    {
        private readonly AzureMetadata _azureMetadata;
        private readonly List<Host.Configuration.Model.Metrics.MetricDefinition> _metrics = new List<Host.Configuration.Model.Metrics.MetricDefinition>();

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

            return ConfigurationSerializer.Serialize(metricsDeclaration);
        }

        public MetricsDeclarationBuilder WithServiceBusMetric(string metricName = "foo", string metricDescription = "Description for a metric", string queueName = "foo-queue", string serviceBusNamespace = "foo-space", string azureMetricName = "Total")
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
                Aggregation = AggregationType.Average
            };
        }
    }
}