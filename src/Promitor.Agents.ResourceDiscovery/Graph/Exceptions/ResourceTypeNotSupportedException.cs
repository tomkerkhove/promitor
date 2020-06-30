using System;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Graph.Exceptions
{
    public class ResourceTypeNotSupportedException : NotSupportedException
    {
        public ResourceTypeNotSupportedException(ResourceType resourceType)
            : base($"Resource type '{resourceType}' is not supported")
        {
            ResourceType = resourceType;
        }

        public ResourceType ResourceType { get; }
    }
}