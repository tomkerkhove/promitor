using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class AutomationAccountDeserializerTests : ResourceDeserializerTest<AutomationAccountDeserializer>
    {
        private readonly AutomationAccountDeserializer _deserializer;

        public AutomationAccountDeserializerTests()
        {
            _deserializer = new AutomationAccountDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AutomationAccountNameSupplied_SetsInstanceName()
        {
            const string automationAccountName = "promitor-automation";
            YamlAssert.PropertySet<AutomationAccountResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"accountName: {automationAccountName}",
                automationAccountName,
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_AutomationAccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<AutomationAccountResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_RunbookNameSupplied_SetsInstanceName()
        {
            const string runbookName = "secret-rotation-runbook";
            YamlAssert.PropertySet<AutomationAccountResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"runbookName: {runbookName}",
                runbookName,
                r => r.RunbookName);
        }

        [Fact]
        public void Deserialize_RunbookNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<AutomationAccountResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.RunbookName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new AutomationAccountDeserializer(Logger);
        }
    }
}