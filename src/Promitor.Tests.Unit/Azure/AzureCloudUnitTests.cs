using System;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureCloudUnitTests : UnitTest
    {
        [Fact]
        public void ValidateMetricsClientAudience_ForUnsupportedCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => azureCloud.ValidateMetricsClientAudience());
        }
    }
}
