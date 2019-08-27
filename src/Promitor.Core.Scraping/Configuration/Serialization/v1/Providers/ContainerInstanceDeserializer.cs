﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerInstanceDeserializer : ResourceDeserializer
    {
        private const string ContainerGroupTag = "containerGroup";

        public ContainerInstanceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var containerGroup = node.GetString(ContainerGroupTag);

            return new ContainerInstanceResourceV1
            {
                ContainerGroup = containerGroup
            };
        }
    }
}
