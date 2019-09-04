using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    public class StorageQueueDeserializerTests : ResourceDeserializerTest
    {
        private readonly Mock<IDeserializer<SecretV1>> _secretDeserializer;

        private readonly StorageQueueDeserializer _deserializer;

        public StorageQueueDeserializerTests()
        {
            _secretDeserializer = new Mock<IDeserializer<SecretV1>>();

            _deserializer = new StorageQueueDeserializer(_secretDeserializer.Object, NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            YamlAssert.PropertySet<StorageQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "accountName: promitor-acct",
                "promitor-acct",
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<StorageQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_QueueNameSupplied_SetsQueueName()
        {
            YamlAssert.PropertySet<StorageQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "queueName: orders",
                "orders",
                r => r.QueueName);
        }

        [Fact]
        public void Deserialize_QueueNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<StorageQueueResourceV1, AzureResourceDefinitionV1>(
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

            var secret = new SecretV1();
            _secretDeserializer.Setup(d => d.Deserialize(sasTokenNode)).Returns(secret);

            // Act
            var resource = (StorageQueueResourceV1)_deserializer.Deserialize(node);

            // Assert
            Assert.Same(secret, resource.SasToken);
        }

        [Fact]
        public void Deserialize_SasTokenNotSupplied_Null()
        {
            YamlAssert.PropertyNull<StorageQueueResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.SasToken);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new StorageQueueDeserializer(new Mock<IDeserializer<SecretV1>>().Object, NullLogger.Instance);
        }
    }
}
