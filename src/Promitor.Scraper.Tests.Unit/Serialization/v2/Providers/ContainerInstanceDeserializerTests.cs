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
    public class ContainerInstanceDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly ContainerInstanceDeserializer _deserializer;

        public ContainerInstanceDeserializerTests()
        {
            _deserializer = new ContainerInstanceDeserializer(new Mock<ILogger>().Object);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new ContainerInstanceDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_ContainerGroupSupplied_SetsContainerGroup()
        {
            DeserializerTestHelpers.AssertPropertySet<ContainerInstanceResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "containerGroup: promitor-group",
                "promitor-group",
                c => c.ContainerGroup);
        }

        [Fact]
        public void Deserialize_ContainerGroupNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<ContainerInstanceResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-resource-group",
                c => c.ContainerGroup);
        }
    }
}
