using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class SqlManagedInstanceDeserializerTests : ResourceDeserializerTest<SqlManagedInstanceDeserializer>
    {
        private readonly SqlManagedInstanceDeserializer _deserializer;

        public SqlManagedInstanceDeserializerTests()
        {
            _deserializer = new SqlManagedInstanceDeserializer(NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_InstanceNameSupplied_SetsDatabaseName()
        {
            YamlAssert.PropertySet<SqlManagedInstanceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "instanceName: promitor-instance",
                "promitor-instance",
                c => c.InstanceName);
        }

        [Fact]
        public void Deserialize_InstanceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlManagedInstanceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.InstanceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SqlDatabaseDeserializer(NullLogger.Instance);
        }
    }
}