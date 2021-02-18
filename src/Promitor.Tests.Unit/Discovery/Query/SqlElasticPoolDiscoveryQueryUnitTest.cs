using System;
using System.ComponentModel;
using Bogus;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class SqlElasticPoolDiscoveryQueryUnitTest
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void GetParentResourceNameFromResourceUri_ValidResourceUri_GetsServerName()
        {
            // Arrange
            var serverName = _faker.Name.FirstName();
            var resourceUri = $"/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/promitor/providers/Microsoft.Sql/servers/{serverName}/elasticpools/promitor-pool";
            var elasticPoolDiscoveryQuery = new SqlElasticPoolDiscoveryQuery();

            // Act
            var foundServerName = elasticPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlElasticPoolDiscoveryQuery.ServerSectionInResourceUri, resourceUri);

            // Assert
            Assert.Equal(serverName, foundServerName);
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_NoResourceUri_ThrowsArgumentException()
        {
            // Arrange
            string resourceUri = null;
            var elasticPoolDiscoveryQuery = new SqlElasticPoolDiscoveryQuery();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => elasticPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlElasticPoolDiscoveryQuery.ServerSectionInResourceUri, resourceUri));
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_EmptyResourceUri_ThrowsArgumentException()
        {
            // Arrange
            var resourceUri = string.Empty;
            var elasticPoolDiscoveryQuery = new SqlElasticPoolDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => elasticPoolDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlElasticPoolDiscoveryQuery.ServerSectionInResourceUri, resourceUri));
        }
    }
}
