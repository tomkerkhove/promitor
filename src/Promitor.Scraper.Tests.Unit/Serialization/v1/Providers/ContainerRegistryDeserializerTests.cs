using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
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
            DeserializerTestHelpers.AssertPropertySet<ContainerRegistryResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "registryName: promitor-registry",
                "promitor-registry",
                c => c.RegistryName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ContainerRegistryResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.RegistryName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ContainerRegistryDeserializer(new Mock<ILogger>().Object);
        }
    }
}
