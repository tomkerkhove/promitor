using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class VirtualMachineScaleSetDeserializerTests : ResourceDeserializerTest<VirtualMachineScaleSetDeserializer>
    {
        private readonly VirtualMachineScaleSetDeserializer _deserializer;

        public VirtualMachineScaleSetDeserializerTests()
        {
            _deserializer = new VirtualMachineScaleSetDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_ScaleSetNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<VirtualMachineScaleSetResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "scaleSetName: promitor-vmss",
                "promitor-vmss",
                r => r.ScaleSetName);
        }

        [Fact]
        public void Deserialize_ScaleSetNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<VirtualMachineScaleSetResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ScaleSetName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new VirtualMachineScaleSetDeserializer(Logger);
        }
    }
}
