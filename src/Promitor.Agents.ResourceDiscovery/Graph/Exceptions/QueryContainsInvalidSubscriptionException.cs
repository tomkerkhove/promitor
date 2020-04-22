using System;
using System.Collections.Generic;
using GuardNet;

namespace Promitor.Agents.ResourceDiscovery.Graph.Exceptions
{
    public class QueryContainsInvalidSubscriptionException : Exception
    {
        public List<string> QueriedSubscriptions { get; }

        public QueryContainsInvalidSubscriptionException(List<string> queriedSubscriptions)
            : base($"Unable to discover resources due to an invalid subscription that was specified ({string.Join(",", queriedSubscriptions)})")
        {
            Guard.NotNull(queriedSubscriptions, nameof(queriedSubscriptions));

            QueriedSubscriptions = queriedSubscriptions;
        }
    }
}
