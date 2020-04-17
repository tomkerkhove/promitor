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
    public class SqlServerDeserializerTests : ResourceDeserializerTest<SqlServerDeserializer>
    {
        private readonly SqlServerDeserializer _deserializer;

        public SqlServerDeserializerTests()
        {
            _deserializer = new SqlServerDeserializer(NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            const string serverName = "promitor-server";

            YamlAssert.PropertySet<SqlServerResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"serverName: {serverName}",
                serverName,
                c => c.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlServerResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.ServerName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SqlDatabaseDeserializer(NullLogger.Instance);
        }
    }
}