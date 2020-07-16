using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class LogicAppDeserializerTests : ResourceDeserializerTest<LogicAppDeserializer>
    {
        private readonly LogicAppDeserializer _deserializer;

        public LogicAppDeserializerTests()
        {
            _deserializer = new LogicAppDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_RegistryNameSupplied_SetsRegistryName()
        {
            const string expectedWorkflowName = "promitor-workflow";
            YamlAssert.PropertySet<LogicAppResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"workflowName: {expectedWorkflowName}",
                expectedWorkflowName,
                c => c.WorkflowName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<LogicAppResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.WorkflowName);
        }

        [Fact]
        public void Deserialize_RegistryNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "workflowName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new LogicAppDeserializer(Logger);
        }
    }
}
