using System;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class BlobStorageDiscoveryQueryUnitTest : UnitTest
    {
        [Fact]
        public void ParseResults_ValidRow_Succeeds()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            var resourceGroup = "storage-rg";
            var accountName = "mystorageaccount";
            var row = JArray.FromObject(new object[]
            {
                subscriptionId,
                resourceGroup,
                "microsoft.storage/storageaccounts",
                accountName
            });
            var query = new BlobStorageDiscoveryQuery();

            // Act
            var result = query.ParseResults(row);

            // Assert
            var blobDef = Assert.IsType<BlobStorageResourceDefinition>(result);
            Assert.Equal(subscriptionId, blobDef.SubscriptionId);
            Assert.Equal(resourceGroup, blobDef.ResourceGroupName);
            Assert.Equal(accountName, blobDef.AccountName);
            Assert.Equal(accountName, blobDef.ResourceName);
        }

        [Fact]
        public void ParseResults_NullRow_Throws()
        {
            // Arrange
            var query = new BlobStorageDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => query.ParseResults(null));
        }
    }
}
