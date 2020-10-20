using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class KubernetesServiceDeserializerTests : ResourceDeserializerTest<KubernetesServiceDeserializer>
    {
        private readonly KubernetesServiceDeserializer _deserializer;

        public KubernetesServiceDeserializerTests()
        {
            _deserializer = new KubernetesServiceDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ClusterNameSupplied_SetsClusterName()
        {
            const string clusterName = "promitor-aks";
            YamlAssert.PropertySet<KubernetesServiceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"clusterName: {clusterName}",
                clusterName,
                r => r.ClusterName);
        }

        [Fact]
        public void Deserialize_ClusterNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<KubernetesServiceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ClusterName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new KubernetesServiceDeserializer(Logger);
        }
    }
}