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
    public class SqlElasticPoolDeserializerTests : ResourceDeserializerTest<SqlElasticPoolDeserializer>
    {
        private readonly SqlElasticPoolDeserializer _deserializer = new(NullLogger.Instance);

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            YamlAssert.PropertySet<SqlElasticPoolResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "serverName: promitor-sql-server",
                "promitor-sql-server",
                c => c.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlElasticPoolResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "serverName");
        }

        [Fact]
        public void Deserialize_PoolNameSupplied_SetsPoolName()
        {
            YamlAssert.PropertySet<SqlElasticPoolResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "poolName: promitor-elastic-pool",
                "promitor-elastic-pool",
                c => c.PoolName);
        }

        [Fact]
        public void Deserialize_PoolNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlElasticPoolResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.PoolName);
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

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SqlElasticPoolDeserializer(NullLogger.Instance);
        }
    }
}