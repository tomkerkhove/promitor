﻿using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class VirtualMachineDeserializerTests : ResourceDeserializerTest
    {
        private readonly VirtualMachineDeserializer _deserializer;

        public VirtualMachineDeserializerTests()
        {
            _deserializer = new VirtualMachineDeserializer(NullLogger.Instance);
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

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new VirtualMachineDeserializer(NullLogger.Instance);
        }
    }
}
