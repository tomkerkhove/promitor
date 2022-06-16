using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class LoadBalancerDeserializerTests : ResourceDeserializerTest<LoadBalancerDeserializer>
    {
        private readonly LoadBalancerDeserializer _deserializer;

        public LoadBalancerDeserializerTests()
        {
            _deserializer = new LoadBalancerDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_LoadBalancerNameSupplied_SetsVaultName()
        {
            const string loadBalancerName = "promitor-load-balancer";
            YamlAssert.PropertySet<LoadBalancerResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"loadBalancerName: {loadBalancerName}",
                loadBalancerName,
                r => r.LoadBalancerName);
        }

        [Fact]
        public void Deserialize_LoadBalancerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<LoadBalancerResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.LoadBalancerName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new LoadBalancerDeserializer(Logger);
        }
    }
}