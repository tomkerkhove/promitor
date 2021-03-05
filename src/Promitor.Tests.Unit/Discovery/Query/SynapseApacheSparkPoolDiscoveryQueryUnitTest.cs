using System;
using System.ComponentModel;
using Bogus;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class SynapseApacheSparkPoolDiscoveryQueryUnitTest
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void GetParentResourceNameFromResourceUri_ValidResourceUri_GetsServerName()
        {
            // Arrange
            var workspaceName = _faker.Name.FirstName();
            var resourceUri = $"/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/promitor-sources/providers/Microsoft.Synapse/workspaces/{workspaceName}/bigDataPools/sparkpool";
            var apacheSparkPoolDiscoveryQuery = new SynapseApacheSparkPoolDiscoveryQuery();

            // Act
            var foundWorkspaceName = apacheSparkPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseApacheSparkPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri);

            // Assert
            Assert.Equal(workspaceName, foundWorkspaceName);
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_NoResourceUri_ThrowsArgumentException()
        {
            // Arrange
            string resourceUri = null;
            var apacheSparkPoolDiscoveryQuery = new SynapseApacheSparkPoolDiscoveryQuery();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => apacheSparkPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseApacheSparkPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri));
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_EmptyResourceUri_ThrowsArgumentException()
        {
            // Arrange
            var resourceUri = string.Empty;
            var apacheSparkPoolDiscoveryQuery = new SynapseApacheSparkPoolDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => apacheSparkPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseApacheSparkPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri));
        }
    }
}
