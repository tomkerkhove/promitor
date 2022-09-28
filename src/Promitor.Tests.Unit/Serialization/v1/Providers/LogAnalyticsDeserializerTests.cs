using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class LogAnalyticsDeserializerTests : ResourceDeserializerTest<LogAnalyticsDeserializer>
    {
        private readonly LogAnalyticsDeserializer _deserializer;

        public LogAnalyticsDeserializerTests()
        {
            _deserializer = new LogAnalyticsDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            const string workspaceId = "123-456-789";
            YamlAssert.PropertySet<LogAnalyticsResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"workspaceId: {workspaceId}",
                workspaceId,
                r => r.WorkspaceId);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<LogAnalyticsResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.WorkspaceId);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new LogAnalyticsDeserializer(Logger);
        }
    }
}