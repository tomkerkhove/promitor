using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class AutomationAccountResourceDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.automation/automationaccounts" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };
        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var runbookName = string.Empty; // We don't use location since we want all info
            var resource = new AutomationAccountResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), resultRowEntry[3]?.ToString(), runbookName);
            return resource;
        }
    }
}
