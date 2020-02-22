﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="ApiManagementDeserializer" /> resource.
    /// </summary>
    public class ApiManagementDeserializer : ResourceDeserializer<ApiManagementResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManagementDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ApiManagementDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override ApiManagementResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var instanceName = node.GetString("instanceName");
            var locationName = node.GetString("locationName");

            return new ApiManagementResourceV1
            {
                InstanceName = instanceName,
                LocationName = locationName
            };
        }
    }
}
