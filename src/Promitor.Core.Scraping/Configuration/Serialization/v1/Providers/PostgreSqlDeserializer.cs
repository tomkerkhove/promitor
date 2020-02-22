﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PostgreSqlDeserializer : ResourceDeserializer<PostgreSqlResourceV1>
    {
        private const string ServerNameTag = "serverName";

        public PostgreSqlDeserializer(ILogger<PostgreSqlDeserializer> logger) : base(logger)
        {
        }

        protected override PostgreSqlResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var serverName = node.GetString(ServerNameTag);

            return new PostgreSqlResourceV1
            {
                ServerName = serverName
            };
        }
    }
}
