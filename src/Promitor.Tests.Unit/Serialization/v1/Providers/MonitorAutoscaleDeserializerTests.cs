using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class MonitorAutoscaleDeserializerTests : ResourceDeserializerTest<MonitorAutoscaleDeserializer>
    {
        private readonly MonitorAutoscaleDeserializer _deserializer;

        public MonitorAutoscaleDeserializerTests()
        {
            _deserializer = new MonitorAutoscaleDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AutoscaleSettingsNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<MonitorAutoscaleResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "autoscaleSettingsName: promitor-application-gateway",
                "promitor-application-gateway",
                r => r.AutoscaleSettingsName);
        }

        [Fact]
        public void Deserialize_AutoscaleSettingsNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<MonitorAutoscaleResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AutoscaleSettingsName);
        }

        [Fact]
        public void Deserialize_AutoscaleSettingsNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "autoscaleSettingsName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new MonitorAutoscaleDeserializer(Logger);
        }
    }
}
