using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1
{
    /// <summary>
    /// This class contains tests that run the full end-to-end serialization and deserialization
    /// process to try to catch anything that the individual unit tests for the deserializers haven't
    /// caught.
    /// </summary>
    [Category("Unit")]
    public class V1SerializationTests : UnitTest
    {
        private readonly V1Deserializer _v1Deserializer;
        private readonly ConfigurationSerializer _configurationSerializer;
        private readonly MetricsDeclarationV1 _metricsDeclaration;
        private readonly IErrorReporter _errorReporter = new ErrorReporter();

        public V1SerializationTests()
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();

            _v1Deserializer = V1DeserializerFactory.CreateDeserializer();
            _configurationSerializer = new ConfigurationSerializer(NullLogger<ConfigurationSerializer>.Instance, mapper, _v1Deserializer);

            _metricsDeclaration = new MetricsDeclarationV1
            {
                AzureMetadata = new AzureMetadataV1
                {
                    TenantId = "tenant",
                    SubscriptionId = "subscription",
                    ResourceGroupName = "promitor-group"
                },
                MetricDefaults = new MetricDefaultsV1
                {
                    Aggregation = new AggregationV1
                    {
                        Interval = TimeSpan.FromMinutes(7)
                    },
                    Scraping = new ScrapingV1
                    {
                        Schedule = "1 2 3 4 5"
                    },
                    Labels = new Dictionary<string, string>
                    {
                        {"geo", "china"}
                    }
                },
                Metrics = new List<MetricDefinitionV1>
                {
                    new()
                    {
                        Name = "promitor_demo_generic_queue_size",
                        Description = "Amount of active messages of the 'orders' queue (determined with Generic provider)",
                        ResourceType = ResourceType.Generic,
                        Labels = new Dictionary<string, string>
                        {
                            {"app", "promitor"}
                        },
                        AzureMetricConfiguration = new AzureMetricConfigurationV1
                        {
                            MetricName = "ActiveMessages",
                            Aggregation = new MetricAggregationV1
                            {
                                Type = AggregationType.Average
                            }
                        },
                        Resources = new List<AzureResourceDefinitionV1>
                        {
                            new GenericResourceV1
                            {
                                ResourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging",
                                Filter = "EntityName eq 'orders'"
                            },
                            new GenericResourceV1
                            {
                                ResourceUri = "Microsoft.ServiceBus/namespaces/promitor-messaging",
                                Filter = "EntityName eq 'accounts'"
                            }
                        }
                    },
                    new()
                    {
                        Name = "promitor_demo_servicebusqueue_queue_size",
                        Description = "Amount of active messages of the 'orders' queue (determined with ServiceBusNamespace provider)",
                        ResourceType = ResourceType.ServiceBusNamespace,
                        AzureMetricConfiguration = new AzureMetricConfigurationV1
                        {
                            MetricName = "ActiveMessages",
                            Aggregation = new MetricAggregationV1
                            {
                                Type = AggregationType.Average,
                                Interval = TimeSpan.FromMinutes(15)
                            }
                        },
                        Scraping = new ScrapingV1
                        {
                            Schedule = "5 4 3 2 1"
                        },
                        Resources = new List<AzureResourceDefinitionV1>
                        {
                            new ServiceBusNamespaceResourceV1
                            {
                                Namespace = "promitor-messaging",
                                QueueName = "orders",
                                ResourceGroupName = "promitor-demo-group"
                            }
                        },
                        ResourceDiscoveryGroups = new List<AzureResourceDiscoveryGroupDefinitionV1>
                        {
                            new()
                            {
                                Name="example-resource-collection"
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void Deserialize_SerializedModel_CanDeserialize()
        {
            // This test creates a v1 model, serializes it to yaml, and then verifies that
            // the V1Deserializer can deserialize it.

            // Arrange
            var yaml = _configurationSerializer.Serialize(_metricsDeclaration);

            // Act
            var deserializedModel = _v1Deserializer.Deserialize(YamlUtils.CreateYamlNode(yaml), _errorReporter);

            // Assert
            Assert.NotNull(deserializedModel);
            Assert.Equal("tenant", deserializedModel.AzureMetadata.TenantId);
            Assert.Equal("subscription", deserializedModel.AzureMetadata.SubscriptionId);
            Assert.Equal("promitor-group", deserializedModel.AzureMetadata.ResourceGroupName);
            Assert.Equal(TimeSpan.FromMinutes(7), deserializedModel.MetricDefaults.Aggregation.Interval);
            Assert.Equal("1 2 3 4 5", deserializedModel.MetricDefaults.Scraping.Schedule);
            Assert.Equal("china", deserializedModel.MetricDefaults.Labels["geo"]);

            // Check first metric
            Assert.Equal("promitor_demo_generic_queue_size", deserializedModel.Metrics.ElementAt(0).Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with Generic provider)", deserializedModel.Metrics.ElementAt(0).Description);
            Assert.Equal(ResourceType.Generic, deserializedModel.Metrics.ElementAt(0).ResourceType);
            Assert.Equal(new Dictionary<string, string> { { "app", "promitor" } }, deserializedModel.Metrics.ElementAt(0).Labels);
            Assert.Equal("ActiveMessages", deserializedModel.Metrics.ElementAt(0).AzureMetricConfiguration.MetricName);
            Assert.Equal(AggregationType.Average, deserializedModel.Metrics.ElementAt(0).AzureMetricConfiguration.Aggregation.Type);
            Assert.Equal(2, deserializedModel.Metrics.ElementAt(0).Resources.Count);

            var genericResource1 = Assert.IsType<GenericResourceV1>(deserializedModel.Metrics.ElementAt(0).Resources.ElementAt(0));
            Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", genericResource1.ResourceUri);
            Assert.Equal("EntityName eq 'orders'", genericResource1.Filter);

            var genericResource2 = Assert.IsType<GenericResourceV1>(deserializedModel.Metrics.ElementAt(0).Resources.ElementAt(1));
            Assert.Equal("Microsoft.ServiceBus/namespaces/promitor-messaging", genericResource2.ResourceUri);
            Assert.Equal("EntityName eq 'accounts'", genericResource2.Filter);

            // Check second metric
            Assert.Equal("promitor_demo_servicebusqueue_queue_size", deserializedModel.Metrics.ElementAt(1).Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with ServiceBusNamespace provider)", deserializedModel.Metrics.ElementAt(1).Description);
            Assert.Equal(ResourceType.ServiceBusNamespace, deserializedModel.Metrics.ElementAt(1).ResourceType);
            Assert.Null(deserializedModel.Metrics.ElementAt(1).Labels);
            Assert.Equal(TimeSpan.FromMinutes(15), deserializedModel.Metrics.ElementAt(1).AzureMetricConfiguration.Aggregation.Interval);
            Assert.Equal("5 4 3 2 1", deserializedModel.Metrics.ElementAt(1).Scraping.Schedule);

            Assert.Single(deserializedModel.Metrics.ElementAt(1).Resources);
            var serviceBusQueueResource = Assert.IsType<ServiceBusNamespaceResourceV1>(deserializedModel.Metrics.ElementAt(1).Resources.ElementAt(0));
            Assert.Equal("promitor-messaging", serviceBusQueueResource.Namespace);
            Assert.Equal("orders", serviceBusQueueResource.QueueName);
            Assert.Equal("promitor-demo-group", serviceBusQueueResource.ResourceGroupName);
            Assert.NotNull(deserializedModel.Metrics.ElementAt(1).ResourceDiscoveryGroups);
            Assert.Single(deserializedModel.Metrics.ElementAt(1).ResourceDiscoveryGroups);
            var resourceDiscoveryGroup =deserializedModel.Metrics.ElementAt(1).ResourceDiscoveryGroups.ElementAt(0);
            Assert.Equal("example-resource-collection", resourceDiscoveryGroup.Name);
        }

        [Fact]
        public void Deserialize_SerializedYaml_CanDeserializeToRuntimeModel()
        {
            // Arrange
            var yaml = _configurationSerializer.Serialize(_metricsDeclaration);

            // Act
            var runtimeModel = _configurationSerializer.Deserialize(yaml, _errorReporter);

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
            Assert.Equal(ResourceType.ServiceBusNamespace, secondMetric.ResourceType);
            Assert.Equal("promitor_demo_servicebusqueue_queue_size", secondMetric.PrometheusMetricDefinition.Name);
            Assert.Equal("Amount of active messages of the 'orders' queue (determined with ServiceBusNamespace provider)", secondMetric.PrometheusMetricDefinition.Description);
            Assert.Collection(secondMetric.Resources,
                r =>
                {
                    var definition = Assert.IsType<ServiceBusNamespaceResourceDefinition>(r);
                    Assert.Equal("promitor-messaging", definition.Namespace);
                    Assert.Equal("orders", definition.QueueName);
                    Assert.Equal("promitor-demo-group", definition.ResourceGroupName);
                });
            Assert.NotNull(secondMetric.ResourceDiscoveryGroups);
            Assert.Single(secondMetric.ResourceDiscoveryGroups);
            var resourceDiscoveryGroup = secondMetric.ResourceDiscoveryGroups.First();
            Assert.Equal("example-resource-collection", resourceDiscoveryGroup.Name);
        }
    }
}
