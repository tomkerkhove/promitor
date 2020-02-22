﻿using System.ComponentModel;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class VirtualMachineDeserializerTests : ResourceDeserializerTest<VirtualMachineDeserializer>
    {
        private readonly VirtualMachineDeserializer _deserializer;

        public VirtualMachineDeserializerTests()
        {
            _deserializer = new VirtualMachineDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_VirtualMachineNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<VirtualMachineResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "virtualMachineName: promitor-vm",
                "promitor-vm",
                r => r.VirtualMachineName);
        }

        [Fact]
        public void Deserialize_VirtualMachineNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<VirtualMachineResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.VirtualMachineName);
        }

        [Fact]
        public void Deserialize_VirtualMachineNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");
            var errorReporter = new Mock<IErrorReporter>();

            // Act
            _deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            errorReporter.Verify(r => r.ReportError(node, It.Is<string>(s => s.Contains("virtualMachineName"))));
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new VirtualMachineDeserializer(Logger);
        }
    }
}
