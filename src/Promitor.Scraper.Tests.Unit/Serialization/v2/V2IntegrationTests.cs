﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Xunit;
using ResourceType = Promitor.Core.Scraping.Configuration.Model.ResourceType;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2
{
    [Category("Unit")]
    public class V2IntegrationTests
    {
        private readonly V2Deserializer _v2Deserializer;
        private readonly ConfigurationSerializer _configurationSerializer;
        private readonly MetricsDeclarationV2 _metricsDeclaration;

        public V2IntegrationTests()
        {
            var logger = new Mock<ILogger>();
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();

            _v2Deserializer = new V2Deserializer(
                new AzureMetadataDeserializer(logger.Object),
                new MetricDefaultsDeserializer(
                    new AggregationDeserializer(logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    logger.Object),
                new MetricDefinitionDeserializer(
                    new AzureMetricConfigurationDeserializer(
                        new MetricAggregationDeserializer(logger.Object),
                        logger.Object),
                    new ScrapingDeserializer(logger.Object),
                    new AzureResourceDeserializerFactory(new SecretDeserializer(logger.Object), logger.Object),
                    logger.Object),
                logger.Object);
            _configurationSerializer = new ConfigurationSerializer(logger.Object, mapper, _v2Deserializer);

            _metricsDeclaration = new MetricsDeclarationV2
            {
                AzureMetadata = new AzureMetadataV2
                {
                    TenantId = "tenant",
                    SubscriptionId = "subscription",
                    ResourceGroupName = "promitor-group"
                },
                MetricDefaults = new MetricDefaultsV2
                {
                    Aggregation = new AggregationV2
                    {
                        Interval = TimeSpan.FromMinutes(7)
                    },
                    Scraping = new ScrapingV2
                    {
                        Schedule = "1 2 3 4 5"
                    }
                },
                Metrics = new List<MetricDefinitionV2>
                {
                    new MetricDefinitionV2
                    {
                        Name = "promitor_demo_generic_queue_size",
                        Description = "Amount of active messages of the 'orders' queue (determined with Generic provider)",
                        ResourceType = ResourceType.Generic,
                        Labels = new Dictionary<string, string>
                        {
                            {"app", "promitor"}
                        },
                        AzureMetricConfiguration = new AzureMetricConfigurationV2
                        {
                            MetricName = "ActiveMessages",
                            Aggregation = new MetricAggregationV2
                            {
                                Type = AggregationType.Average
                            }
                        },
                        Resources = new List<AzureResourceDefinitionV2>
                        {
                            new GenericResourceV2
                            {
                                ResourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging",
                                Filter = "EntityName eq 'orders'"
                            },
                            new GenericResourceV2
                            {
                                ResourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging",
                                Filter = "EntityName eq 'accounts'"
                            }
                        }
                    },
                    new MetricDefinitionV2
                    {
                        Name = "promitor_demo_servicebusqueue_queue_size",
                        Description = "Amount of active messages of the 'orders' queue (determined with ServiceBusQueue provider)",
                        ResourceType = ResourceType.ServiceBusQueue,
                        AzureMetricConfiguration = new AzureMetricConfigurationV2
                        {
                            MetricName = "ActiveMessages",
                            Aggregation = new MetricAggregationV2
                            {
                                Type = AggregationType.Average,
                                Interval = TimeSpan.FromMinutes(15)
                            }
                        },
                        Scraping = new ScrapingV2
                        {
                            Schedule = "5 4 3 2 1"
                        },
                        Resources = new List<AzureResourceDefinitionV2>
                        {
                            new ServiceBusQueueResourceV2
                            {
                                Namespace = "promitor-messaging",
                                QueueName = "orders",
                                ResourceGroupName = "promitor-demo-group"
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void CanDeserializeSerializedModel()
        {
            // This test creates a v2 model, serializes it to yaml, and then verifies that
            // the V2Deserializer can deserialize it.

            // Arrange
            var yaml = _configurationSerializer.Serialize(_metricsDeclaration);

            // Act
            var deserializedModel = _v2Deserializer.Deserialize(YamlUtils.CreateYamlNode(yaml));

            // Assert
            Assert.NotNull(deserializedModel);
            Assert.Equal("tenant", deserializedModel.AzureMetadata.TenantId);
            Assert.Equal("subscription", deserializedModel.AzureMetadata.SubscriptionId);
            Assert.Equal("promitor-group", deserializedModel.AzureMetadata.ResourceGroupName);
            Assert.Equal(TimeSpan.FromMinutes(7), deserializedModel.MetricDefaults.Aggregation.Interval);
            Assert.Equal("1 2 3 4 5", deserializedModel.MetricDefaults.Scraping.Schedule);

            // Check first metric
            Assert.Equal("promitor_demo_generic_queue_size", deserializedModel.Metrics.ElementAt(0).Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with Generic provider)", deserializedModel.Metrics.ElementAt(0).Description);
            Assert.Equal(ResourceType.Generic, deserializedModel.Metrics.ElementAt(0).ResourceType);
            Assert.Equal(new Dictionary<string, string> {{"app", "promitor"}}, deserializedModel.Metrics.ElementAt(0).Labels);
            Assert.Equal("ActiveMessages", deserializedModel.Metrics.ElementAt(0).AzureMetricConfiguration.MetricName);
            Assert.Equal(AggregationType.Average, deserializedModel.Metrics.ElementAt(0).AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(2, deserializedModel.Metrics.ElementAt(0).Resources.Count);

            var genericResource1 = Assert.IsType<GenericResourceV2>(deserializedModel.Metrics.ElementAt(0).Resources.ElementAt(0));
            Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", genericResource1.ResourceUri);
            Assert.Equal("EntityName eq 'orders'", genericResource1.Filter);

            var genericResource2 = Assert.IsType<GenericResourceV2>(deserializedModel.Metrics.ElementAt(0).Resources.ElementAt(1));
            Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", genericResource2.ResourceUri);
            Assert.Equal("EntityName eq 'accounts'", genericResource2.Filter);

            // Check second metric
            Assert.Equal("promitor_demo_servicebusqueue_queue_size", deserializedModel.Metrics.ElementAt(1).Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with ServiceBusQueue provider)", deserializedModel.Metrics.ElementAt(1).Description);
            Assert.Equal(ResourceType.ServiceBusQueue, deserializedModel.Metrics.ElementAt(1).ResourceType);
            Assert.Null(deserializedModel.Metrics.ElementAt(1).Labels);
            Assert.Equal(TimeSpan.FromMinutes(15), deserializedModel.Metrics.ElementAt(1).AzureMetricConfiguration.Aggregation.Interval);
            Assert.Equal("5 4 3 2 1", deserializedModel.Metrics.ElementAt(1).Scraping.Schedule);

            Assert.Single(deserializedModel.Metrics.ElementAt(1).Resources);
            var serviceBusQueueResource = Assert.IsType<ServiceBusQueueResourceV2>(deserializedModel.Metrics.ElementAt(1).Resources.ElementAt(0));
            Assert.Equal("promitor-messaging", serviceBusQueueResource.Namespace);
            Assert.Equal("orders", serviceBusQueueResource.QueueName);
            Assert.Equal("promitor-demo-group", serviceBusQueueResource.ResourceGroupName);
        }

        [Fact]
        public void CanDeserializeToRuntimeModel()
        {
            // Arrange
            var yaml = _configurationSerializer.Serialize(_metricsDeclaration);

            // Act
            var runtimeModel = _configurationSerializer.Deserialize(yaml);

            // Assert
            Assert.NotNull(runtimeModel);

            var firstMetric = runtimeModel.Metrics.ElementAt(0);
            Assert.Equal(ResourceType.Generic, firstMetric.ResourceType);
            Assert.Equal("promitor_demo_generic_queue_size", firstMetric.PrometheusMetricDefinition.Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with Generic provider)", firstMetric.PrometheusMetricDefinition.Description);
            Assert.Collection(firstMetric.Resources, 
                r =>
                {
                    var definition = Assert.IsType<GenericAzureResourceDefinition>(r);
                    Assert.Equal("EntityName eq 'orders'", definition.Filter);
                    Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", definition.ResourceUri);
                },
                r =>
                {
                    var definition = Assert.IsType<GenericAzureResourceDefinition>(r);
                    Assert.Equal("EntityName eq 'accounts'", definition.Filter);
                    Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", definition.ResourceUri);
                });

            var secondMetric = runtimeModel.Metrics.ElementAt(1);
            Assert.Equal(ResourceType.ServiceBusQueue, secondMetric.ResourceType);
            Assert.Equal("promitor_demo_servicebusqueue_queue_size", secondMetric.PrometheusMetricDefinition.Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with ServiceBusQueue provider)", secondMetric.PrometheusMetricDefinition.Description);
            Assert.Collection(secondMetric.Resources, 
                r =>
                {
                    var definition = Assert.IsType<ServiceBusQueueResourceDefinition>(r);
                    Assert.Equal("promitor-messaging", definition.Namespace);
                    Assert.Equal("orders", definition.QueueName);
                    Assert.Equal("promitor-demo-group", definition.ResourceGroupName);
                });
        }
    }
}
