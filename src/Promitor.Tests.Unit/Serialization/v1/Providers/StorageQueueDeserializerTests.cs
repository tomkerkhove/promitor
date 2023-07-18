using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class StorageQueueDeserializerTests : ResourceDeserializerTest<StorageQueueDeserializer>
    {
        private readonly Mock<IDeserializer<SecretV1>> _secretDeserializer;
        private readonly Mock<IErrorReporter> _errorReporter = new();

        private readonly StorageQueueDeserializer _deserializer;

        public StorageQueueDeserializerTests()
        {
            _secretDeserializer = new Mock<IDeserializer<SecretV1>>();

            _deserializer = new StorageQueueDeserializer(_secretDeserializer.Object, Logger);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            const string storageAccountName = "promitor-account";
            YamlAssert.PropertySet<StorageQueueResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"accountName: {storageAccountName}",
                storageAccountName,
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
        public void Deserialize_AccountNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "accountName");
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
        public void Deserialize_QueueNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "queueName");
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
            _secretDeserializer.Setup(d => d.Deserialize(sasTokenNode, _errorReporter.Object)).Returns(secret);

            // Act
            var resource = _deserializer.Deserialize(node, _errorReporter.Object);

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

        [Fact]
        public void Deserialize_SasTokenNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "sasToken");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new StorageQueueDeserializer(new Mock<IDeserializer<SecretV1>>().Object, Logger);
        }
    }
}
