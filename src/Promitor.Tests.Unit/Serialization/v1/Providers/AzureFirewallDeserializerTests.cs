
using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class AzureFirewallDeserializerTests : ResourceDeserializerTest<AzureFirewallDeserializer>
	{
        private readonly AzureFirewallDeserializer _deserializer;

	public AzureFirewallDeserializerTests()
	{
            _deserializer = new AzureFirewallDeserializer(Logger);
	}

        [Fact]
        public void Deserialize_AzureFirewallNameSupplied_SetsAzureFirewallName()
        {
            YamlAssert.PropertySet<AzureFirewallResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "azureFirewallName: promitor-firewall-name",
                "promitor-firewall-name",
                r => r.AzureFirewallName);
        }

        [Fact]
        public void Deserialize_AzureFirewallNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<AzureFirewallResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AzureFirewallName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new AzureFirewallDeserializer(Logger);
        }
    }
}
