
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class CognitiveServicesAccountDeserializerTests : ResourceDeserializerTest<CognitiveServicesAccountDeserializer>
	{
        private readonly CognitiveServicesAccountDeserializer _deserializer;

	public CognitiveServicesAccountDeserializerTests()
	{
            _deserializer = new CognitiveServicesAccountDeserializer(Logger);
	}

        [Fact]
        public void Deserialize_CognitiveServicesAccountNameSupplied_SetsCognitiveServiceAccountlName()
        {
            YamlAssert.PropertySet<CognitiveServicesAccountResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "cognitiveServicesAccountName: promitor-cognitive-services-account-name",
                "promitor-cognitive-services-account-name",
                r => r.CognitiveServicesAccountName);
        }

        [Fact]
        public void Deserialize_CognitiveServicesAccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<CognitiveServicesAccountResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.CognitiveServicesAccountName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new CognitiveServicesAccountDeserializer(Logger);
        }
    }
}
