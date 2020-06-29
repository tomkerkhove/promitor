using System;
using Promitor.Core.Scraping;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public abstract class AppServiceResourceDiscoveryQuery : ResourceDiscoveryQuery
    {
        public (string AppName, string SlotName) DetermineAppDetails(string resourceName)
        {
            if (resourceName.Contains("/"))
            {
                if (resourceName.EndsWith("/"))
                {
                    throw new NotSupportedException("No slot name was provided while the resource name ends with a \"/\"");
                }

                var nameSegments = resourceName.Split("/");
                if (nameSegments.Length != 2)
                {
                    throw new NotSupportedException("Resource name can only contain 1 \"/\" in its name");

                }

                return (nameSegments[0], nameSegments[1]);
            }

            return (resourceName, Defaults.AppServices.SlotName);
        }
    }
}
