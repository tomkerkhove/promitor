
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class CognitiveServicesAccountsDeserializerTests : ResourceDeserializerTest<CognitiveServicesAccountsDeserializer>
	{
        private readonly CognitiveServicesAccountsDeserializer _deserializer;

	public CognitiveServicesAccountsDeserializerTests()
	{
            _deserializer = new CognitiveServicesAccountsDeserializer(Logger);
	}

        [Fact]
        public void Deserialize_CognitiveServicesAccountsNameSupplied_SetsCognitiveServiceAccountslName()
        {
            YamlAssert.PropertySet<CognitiveServicesAccountsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "cognitiveServicesAccountsName: promitor-cognitive-services-accounts-name",
                "promitor-cognitive-services-accounts-name",
                r => r.CognitiveServicesAccountsName);
        }

        [Fact]
        public void Deserialize_CognitiveServicesAccountsNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<CognitiveServicesAccountsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.CognitiveServicesAccountsName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new CognitiveServicesAccountsDeserializer(Logger);
        }
    }
}
