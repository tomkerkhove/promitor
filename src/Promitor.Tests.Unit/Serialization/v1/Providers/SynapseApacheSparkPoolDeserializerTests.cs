using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class SynapseApacheSparkPoolDeserializerTests : ResourceDeserializerTest<SynapseApacheSparkPoolDeserializer>
    {
        private readonly SynapseApacheSparkPoolDeserializer _deserializer = new(NullLogger<SynapseApacheSparkPoolDeserializer>.Instance);

        [Fact]
        public void Deserialize_PoolNameSupplied_SetsPoolName()
        {
            const string expectedPoolName = "promitor-synapse-spark-pool";
            YamlAssert.PropertySet<SynapseApacheSparkPoolResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"poolName: {expectedPoolName}",
                expectedPoolName,
                c => c.PoolName);
        }

        [Fact]
        public void Deserialize_PoolNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SynapseApacheSparkPoolResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.PoolName);
        }

        [Fact]
        public void Deserialize_WorkspaceNameSupplied_SetsWorkspace()
        {
            const string expectedWorkspaceName = "promitor-synapse-workspace";
            YamlAssert.PropertySet<SynapseApacheSparkPoolResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"workspaceName: {expectedWorkspaceName}",
                expectedWorkspaceName,
                c => c.WorkspaceName);
        }

        [Fact]
        public void Deserialize_WorkspaceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SynapseApacheSparkPoolResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.WorkspaceName);
        }

        [Fact]
        public void Deserialize_PoolNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "poolName");
        }

        [Fact]
        public void Deserialize_WorkspaceNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "workspaceName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SynapseApacheSparkPoolDeserializer(NullLogger<SynapseApacheSparkPoolDeserializer>.Instance);
        }
    }
}