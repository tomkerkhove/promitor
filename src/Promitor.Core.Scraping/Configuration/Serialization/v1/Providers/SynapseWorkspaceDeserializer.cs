using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class SynapseWorkspaceDeserializer : ResourceDeserializer<SynapseWorkspaceResourceV1>
    {
        public SynapseWorkspaceDeserializer(ILogger<SynapseWorkspaceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WorkspaceName)
                .IsRequired();
        }
    }
}