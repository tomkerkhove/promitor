using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class AppPlanDeserializerTests : ResourceDeserializerTest<AppPlanDeserializer>
    {
        private readonly AppPlanDeserializer _deserializer;

        public AppPlanDeserializerTests()
        {
            _deserializer = new AppPlanDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AppPlanNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<AppPlanResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "appPlanName: promitor-app-plan",
                "promitor-app-plan",
                r => r.AppPlanName);
        }

        [Fact]
        public void Deserialize_AppPlanNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<AppPlanResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AppPlanName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new AppPlanDeserializer(Logger);
        }
    }
}
