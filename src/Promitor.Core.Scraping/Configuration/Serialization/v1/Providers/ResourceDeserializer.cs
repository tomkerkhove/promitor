using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    ///     A base class for azure resource deserializers that makes sure that any shared
    ///     properties are deserialized correctly for all resources.
    /// </summary>
    public abstract class ResourceDeserializer<TResourceDefinition> : Deserializer<TResourceDefinition>
        where TResourceDefinition : AzureResourceDefinitionV1, new()
    {
        protected ResourceDeserializer(ILogger logger) : base(logger)
        {
            MapOptional(resource => resource.SubscriptionId);
            MapOptional(resource => resource.ResourceGroupName);
        }
    }
}