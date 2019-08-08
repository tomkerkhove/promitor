﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class ContainerInstanceDeserializer : ResourceDeserializer
    {
        private const string ContainerGroupTag = "containerGroup";

        public ContainerInstanceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new ContainerInstanceResourceV2
            {
                ContainerGroup = GetString(node, ContainerGroupTag)
            };
        }
    }
}
