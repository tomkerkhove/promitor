using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class CdnDeserializerTests : ResourceDeserializerTest<CdnDeserializer>
    {
        private readonly CdnDeserializer _deserializer;

        public CdnDeserializerTests()
        {
            _deserializer = new CdnDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_CdnNameSupplied_SetsDbName()
        {
            const string cdnName = "promitor-cdn";
            YamlAssert.PropertySet<CdnResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"cdnName: {cdnName}",
                cdnName,
                c => c.CdnName);
        }

        [Fact]
        public void Deserialize_CdnNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<CdnResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.CdnName);
        }

        [Fact]
        public void Deserialize_DbNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "cdnName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new CdnDeserializer(Logger);
        }
    }
}
