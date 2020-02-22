using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class SqlDatabaseDeserializerTests : ResourceDeserializerTest<SqlDatabaseDeserializer>
    {
        private readonly SqlDatabaseDeserializer _deserializer = new SqlDatabaseDeserializer(NullLogger.Instance);
        private readonly Mock<IErrorReporter> _errorReporter = new Mock<IErrorReporter>();

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            YamlAssert.PropertySet<SqlDatabaseResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "serverName: promitor-sql-server",
                "promitor-sql-server",
                c => c.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlDatabaseResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(r => r.ReportError(node, It.Is<string>(s => s.Contains("serverName"))));
        }

        [Fact]
        public void Deserialize_DatabaseNameSupplied_SetsDatabaseName()
        {
            YamlAssert.PropertySet<SqlDatabaseResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "databaseName: promitor-db",
                "promitor-db",
                c => c.DatabaseName);
        }

        [Fact]
        public void Deserialize_DatabaseNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<SqlDatabaseResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                c => c.DatabaseName);
        }

        [Fact]
        public void Deserialize_DatabaseNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-resource-group");

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(r => r.ReportError(node, It.Is<string>(s => s.Contains("databaseName"))));
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new SqlDatabaseDeserializer(NullLogger.Instance);
        }
    }
}