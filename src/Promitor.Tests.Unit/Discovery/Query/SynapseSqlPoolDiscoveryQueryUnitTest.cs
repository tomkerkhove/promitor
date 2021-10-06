using System;
using System.ComponentModel;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class SynapseSqlPoolDiscoveryQueryUnitTest : UnitTest
    {
        [Fact]
        public void GetParentResourceNameFromResourceUri_ValidResourceUri_GetsServerName()
        {
            // Arrange
            var workspaceName = BogusGenerator.Name.FirstName();
            var resourceUri = $"/subscriptions/63c590b6-4947-4898-92a3-cae91a31b5e4/resourceGroups/promitor-sources/providers/Microsoft.Synapse/workspaces/{workspaceName}/sqlPools/sqlpool";
            var sqlPoolDiscoveryQuery = new SynapseSqlPoolDiscoveryQuery();

            // Act
            var foundWorkspaceName = sqlPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseSqlPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri);

            // Assert
            Assert.Equal(workspaceName, foundWorkspaceName);
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_NoResourceUri_ThrowsArgumentException()
        {
            // Arrange
            string resourceUri = null;
            var sqlPoolDiscoveryQuery = new SynapseSqlPoolDiscoveryQuery();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => sqlPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseSqlPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri));
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_EmptyResourceUri_ThrowsArgumentException()
        {
            // Arrange
            var resourceUri = string.Empty;
            var sqlPoolDiscoveryQuery = new SynapseSqlPoolDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sqlPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SynapseSqlPoolDiscoveryQuery.WorkspaceSectionInResourceUri, resourceUri));
        }
    }
}
