using System;
using System.Collections.Generic;
using System.Linq;
using GuardNet;

namespace Promitor.Agents.ResourceDiscovery.Graph.Exceptions
{
    public class QueryContainsInvalidSubscriptionException : Exception
    {
        public List<string> QueriedSubscriptions { get; }

        public QueryContainsInvalidSubscriptionException(List<string> queriedSubscriptions)
            : base(ComposeExceptionMessage(queriedSubscriptions))
        {
            Guard.NotNull(queriedSubscriptions, nameof(queriedSubscriptions));

            QueriedSubscriptions = queriedSubscriptions;
        }

        private static string ComposeExceptionMessage(List<string> queriedSubscriptions)
        {
            if (queriedSubscriptions?.Any() == true)
            {
                return $"Unable to discover resources due to an invalid subscription that was specified ({string.Join(",", queriedSubscriptions)})";
            }

            return $"Unable to discover resources due to an invalid subscription that was specified while querying discovered subscriptions";
        }
    }
}
