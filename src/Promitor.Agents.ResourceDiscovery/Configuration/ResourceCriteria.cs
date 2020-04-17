using System.Collections.Generic;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class ResourceCriteria
    {
        public Dictionary<string,string> Tags { get; set; } = new Dictionary<string, string>();
        public List<string> Subscriptions { get; set; } = new List<string>();
        public List<string> ResourceGroups { get; set; } = new List<string>();
        public List<string> Regions { get; set; } = new List<string>();
    }
}