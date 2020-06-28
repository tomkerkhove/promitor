namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public abstract class AppServiceResourceDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string DefaultSlotName = "production";

        public (string AppName, string SlotName) DetermineAppDetails(string appName)
        {
            if (appName.Contains("/"))
            {
                var webAppNameSegments = appName.Split("/");
                return (webAppNameSegments[0], webAppNameSegments[1]);
            }

            return (appName, DefaultSlotName);
        }
    }
}
