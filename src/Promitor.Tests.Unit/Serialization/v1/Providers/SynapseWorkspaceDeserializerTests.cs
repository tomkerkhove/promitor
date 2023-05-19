using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class SynapseWorkspaceDeserializerTests : ResourceDeserializerTest<SynapseWorkspaceDeserializer>
    {
        private readonly SynapseWorkspaceDeserializer _deserializer = new(NullLogger<SynapseWorkspaceDeserializer>.Instance);
        
        [Fact]
        public void Deserialize_WorkspaceNameSupplied_SetsWorkspace()
        {
            const string expectedWorkspaceName = "promitor-synapse-workspace";
            YamlAssert.PropertySet<SynapseWorkspaceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"workspaceName: {expectedWorkspaceName}",
                expectedWorkspaceName,
                c => c.WorkspaceName);
        }

        [Fact]
        public void Deserialize_WorkspaceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SynapseWorkspaceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.WorkspaceName);
        }

        [Fact]
        public void Deserialize_WorkspaceNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "workspaceName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SynapseWorkspaceDeserializer(NullLogger<SynapseWorkspaceDeserializer>.Instance);
        }
    }
}