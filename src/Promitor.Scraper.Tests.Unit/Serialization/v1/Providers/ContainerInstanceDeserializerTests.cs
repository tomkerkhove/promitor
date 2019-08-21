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
    public class ContainerInstanceDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly ContainerInstanceDeserializer _deserializer;

        public ContainerInstanceDeserializerTests()
        {
            _deserializer = new ContainerInstanceDeserializer(new Mock<ILogger>().Object);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ContainerInstanceDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_ContainerGroupSupplied_SetsContainerGroup()
        {
            DeserializerTestHelpers.AssertPropertySet<ContainerInstanceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "containerGroup: promitor-group",
                "promitor-group",
                c => c.ContainerGroup);
        }

        [Fact]
        public void Deserialize_ContainerGroupNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ContainerInstanceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-resource-group",
                c => c.ContainerGroup);
        }
    }
}
