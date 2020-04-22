using System;
using System.Collections.Generic;
using GuardNet;

namespace Promitor.Agents.ResourceDiscovery.Graph.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string ApplicationId { get; }
        public List<string> Subscriptions { get; }

        public UnauthorizedException(string applicationId, List<string> subscriptions)
            : base($"Unable to discover resources due to invalid permissions for application with id '{applicationId} while querying subscription(s) {string.Join(",", subscriptions)}")
        {
            Guard.NotNullOrWhitespace(applicationId, nameof(applicationId));
            Guard.NotNull(subscriptions, nameof(subscriptions));

            ApplicationId = applicationId;
            Subscriptions = subscriptions;
        }
    }
}