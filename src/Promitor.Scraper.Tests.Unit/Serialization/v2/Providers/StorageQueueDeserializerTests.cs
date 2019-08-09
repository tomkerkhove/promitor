using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    public class StorageQueueDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly Mock<IDeserializer<SecretV2>> _secretDeserializer;

        private readonly StorageQueueDeserializer _deserializer;

        public StorageQueueDeserializerTests()
        {
            _secretDeserializer = new Mock<IDeserializer<SecretV2>>();

            _deserializer = new StorageQueueDeserializer(_secretDeserializer.Object, new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            DeserializerTestHelpers.AssertPropertySet<StorageQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "accountName: promitor-acct",
                "promitor-acct",
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<StorageQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_QueueNameSupplied_SetsQueueName()
        {
            DeserializerTestHelpers.AssertPropertySet<StorageQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "queueName: orders",
                "orders",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<StorageQueueResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_SasTokenSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"sasToken:
    rawValue: abc123";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var sasTokenNode = (YamlMappingNode)node.Children["sasToken"];

            var secret = new SecretV2();
            _secretDeserializer.Setup(d => d.Deserialize(sasTokenNode)).Returns(secret);

            // Act
            var resource = (StorageQueueResourceV2)_deserializer.Deserialize(node);

            // Assert
            Assert.Same(secret, resource.SasToken);
        }

        [Fact]
        public void Deserialize_SasTokenNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<StorageQueueResourceV2, AzureResourceDefinitionV2, SecretV2>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.SasToken);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new StorageQueueDeserializer(new Mock<IDeserializer<SecretV2>>().Object, new Mock<ILogger>().Object);
        }
    }
}
