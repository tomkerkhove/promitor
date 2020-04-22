using System;
using GuardNet;
using Microsoft.Azure.Management.ResourceGraph.Models;

namespace Promitor.Agents.ResourceDiscovery.Graph.Exceptions
{
    public class ParseException : Exception
    {
        /// <summary>
        ///     Response provided by Azure Resource Graph after running a query
        /// </summary>
        public QueryResponse QueryResponse { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="queryResponse">Response provided by Azure Resource Graph after running a query</param>
        public ParseException(QueryResponse queryResponse)
            : base("Unable to parse Azure Resource Graph query response")
        {
            Guard.NotNull(queryResponse, nameof(queryResponse));

            QueryResponse = queryResponse;
        }
    }
}