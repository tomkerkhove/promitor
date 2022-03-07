using System.ComponentModel;
using Promitor.Core.Contracts;
using Xunit;

namespace Promitor.Tests.Unit.Core
{
    [Category("Unit")]
    public class PagedResultTests
    {
        [Theory]
        [InlineData(1000, 1, 1000)]
        [InlineData(900, 1, 1000)]
        [InlineData(1500, 2, 1000)]
        public void HasMore_NoMoreEntriesAreAvailableInNextPage_ReturnsFalse(long totalRecords, int currentPage, int pageSize)
        {
            // Arrange
            object result = new { foo = "bar"};

            // Act
            var resource = new PagedPayload<object>(result, totalRecords, currentPage, pageSize);

            // Assert
            Assert.False(resource.HasMore);
        }

        [Theory]
        [InlineData(1018, 1, 1000)]
        [InlineData(900, 2, 300)]
        public void HasMore_MoreEntriesAreAvailableInNextPage_ReturnsTrue(long totalRecords, int currentPage, int pageSize)
        {
            // Arrange
            object result = new { foo = "bar" };

            // Act
            var resource = new PagedPayload<object>(result, totalRecords, currentPage, pageSize);

            // Assert
            Assert.True(resource.HasMore);
        }
    }
}
