﻿using System;
using System.Linq;
using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class SynapseApacheSparkPoolDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string WorkspaceSectionInResourceUri = "workspaces/";
        public override string[] ResourceTypes => new[] { "microsoft.synapse/workspaces/bigdatapools" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name", "id" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            // Get workspace name
            var workspaceName = GetParentResourceNameFromResourceUri(WorkspaceSectionInResourceUri, resultRowEntry[4]);
            if (string.IsNullOrWhiteSpace(workspaceName))
            {
                throw new Exception($"Unable to determine workspace name from resource URI '{resultRowEntry[4]}'");
            }

            // Create resource definition
            var resource = new SynapseApacheSparkPoolResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), workspaceName, resultRowEntry[3]?.ToString());
            return resource;
        }
    }
}