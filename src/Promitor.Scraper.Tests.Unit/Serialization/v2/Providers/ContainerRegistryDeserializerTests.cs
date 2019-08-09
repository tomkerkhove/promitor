using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    [Category("Unit")]
    public class ContainerRegistryDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly ContainerRegistryDeserializer _deserializer;

        public ContainerRegistryDeserializerTests()
        {
            _deserializer = new ContainerRegistryDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_RegistryNameSupplied_SetsRegistryName()
        {
            DeserializerTestHelpers.AssertPropertySet<ContainerRegistryResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "registryName: promitor-registry",
                "promitor-registry",
                c => c.RegistryName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ContainerRegistryResourceV2, AzureResourceDefinitionV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.RegistryName);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new ContainerRegistryDeserializer(new Mock<ILogger>().Object);
        }
    }
}
