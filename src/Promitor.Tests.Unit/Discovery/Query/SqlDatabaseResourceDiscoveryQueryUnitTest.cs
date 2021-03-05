using System;
using System.ComponentModel;
using Bogus;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class SqlDatabaseResourceDiscoveryQueryUnitTest
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void GetParentResourceNameFromResourceUri_ValidResourceUri_GetsServerName()
        {
            // Arrange
            var serverName = _faker.Name.FirstName();
            var resourceUri = $"/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/promitor/providers/Microsoft.Sql/servers/{serverName}/databases/promitor-3";
            var sqlDatabaseDiscoveryQuery = new SqlDatabaseDiscoveryQuery();

            // Act
            var foundServerName = sqlDatabaseDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlDatabaseDiscoveryQuery.ServerSectionInResourceUri, resourceUri);

            // Assert
            Assert.Equal(serverName, foundServerName);
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_NoResourceUri_ThrowsArgumentException()
        {
            // Arrange
            string resourceUri = null;
            var sqlDatabaseDiscoveryQuery = new SqlDatabaseDiscoveryQuery();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentException>(() => sqlDatabaseDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlDatabaseDiscoveryQuery.ServerSectionInResourceUri, resourceUri));
        }

        [Fact]
        public void GetParentResourceNameFromResourceUri_EmptyResourceUri_ThrowsArgumentException()
        {
            // Arrange
            var resourceUri = string.Empty;
            var sqlDatabaseDiscoveryQuery = new SqlDatabaseDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => sqlDatabaseDiscoveryQuery.GetParentResourceNameFromResourceUri(SqlDatabaseDiscoveryQuery.ServerSectionInResourceUri, resourceUri));
        }
    }
}
