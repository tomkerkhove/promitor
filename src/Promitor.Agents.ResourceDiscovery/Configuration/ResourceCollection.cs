namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class ResourceCollection
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ResourceCriteria Criteria { get; set; } = new ResourceCriteria();
    }
}