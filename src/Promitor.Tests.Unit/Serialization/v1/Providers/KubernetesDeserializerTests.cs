using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class KubernetesDeserializerTests : ResourceDeserializerTest<KubernetesDeserializer>
    {
        private readonly KubernetesDeserializer _deserializer;

        public KubernetesDeserializerTests()
        {
            _deserializer = new KubernetesDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ClusterNameSupplied_SetsClusterName()
        {
            const string clusterName = "promitor-aks";
            YamlAssert.PropertySet<KubernetesResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"clusterName: {clusterName}",
                clusterName,
                r => r.ClusterName);
        }

        [Fact]
        public void Deserialize_ClusterNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<KubernetesResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ClusterName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new KubernetesDeserializer(Logger);
        }
    }
}