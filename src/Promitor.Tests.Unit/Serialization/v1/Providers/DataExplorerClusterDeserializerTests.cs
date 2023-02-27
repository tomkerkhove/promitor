using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class DataExplorerClusterDeserializerTests : ResourceDeserializerTest<DataExplorerClusterDeserializer>
    {
        private readonly DataExplorerClusterDeserializer _deserializer;

        public DataExplorerClusterDeserializerTests()
        {
            _deserializer = new DataExplorerClusterDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_DataExplorerClusterNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<DataExplorerClusterResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "clusterName: promitor-data-explorer-cluster",
                "promitor-data-explorer-cluster",
                r => r.ClusterName);
        }

        [Fact]
        public void Deserialize_DataExplorerClusterNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<DataExplorerClusterResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ClusterName);
        }

        [Fact]
        public void Deserialize_DataExplorerClusterNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "clusterName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new DataExplorerClusterDeserializer(Logger);
        }
    }
}
