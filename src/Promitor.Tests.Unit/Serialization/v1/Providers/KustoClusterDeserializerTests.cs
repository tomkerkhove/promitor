using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class KustoClusterDeserializerTests : ResourceDeserializerTest<KustoClusterDeserializer>
    {
        private readonly KustoClusterDeserializer _deserializer;

        public KustoClusterDeserializerTests()
        {
            _deserializer = new KustoClusterDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_KustoClusterNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<KustoClusterResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "kustoClusterName: promitor-kusto-cluster",
                "promitor-kusto-cluster",
                r => r.KustoClusterName);
        }

        [Fact]
        public void Deserialize_KustoClusterNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<KustoClusterResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.KustoClusterName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new KustoClusterDeserializer(Logger);
        }
    }
}
