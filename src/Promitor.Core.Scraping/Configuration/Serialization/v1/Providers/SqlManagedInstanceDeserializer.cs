using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="SqlManagedInstanceDeserializer" /> resource.
    /// </summary>
    public class SqlManagedInstanceDeserializer : ResourceDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlManagedInstanceDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlManagedInstanceDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var instanceName = node.GetString("instanceName");

            return new SqlManagedInstanceResourceV1
            {
                InstanceName = instanceName
            };
        }
    }
}
