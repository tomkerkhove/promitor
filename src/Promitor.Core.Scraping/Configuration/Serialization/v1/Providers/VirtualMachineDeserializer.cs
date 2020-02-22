﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class VirtualMachineDeserializer : ResourceDeserializer<VirtualMachineResourceV1>
    {
        private const string VirtualMachineNameTag = "virtualMachineName";

        public VirtualMachineDeserializer(ILogger<VirtualMachineDeserializer> logger) : base(logger)
        {
        }

        protected override VirtualMachineResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var virtualMachineName = node.GetString(VirtualMachineNameTag);

            return new VirtualMachineResourceV1
            {
                VirtualMachineName = virtualMachineName
            };
        }
    }
}
