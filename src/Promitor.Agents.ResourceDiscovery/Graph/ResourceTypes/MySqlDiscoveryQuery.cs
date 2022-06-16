using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class MySqlResourceDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string SingleServerResourceType = "microsoft.dbformysql/servers";
        private const string FlexibleServerResourceType = "microsoft.dbformysql/flexibleservers";

        public override string[] ResourceTypes => new[] { SingleServerResourceType, FlexibleServerResourceType };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            MySqlServerType serverType = MySqlServerType.Single;

            var resourceType = resultRowEntry[2]?.ToString();

            switch (resourceType?.ToLower())
            {
                case SingleServerResourceType:
                    serverType = MySqlServerType.Single;
                    break;
                case FlexibleServerResourceType:
                    serverType = MySqlServerType.Flexible;
                    break;
            }

            var resource = new MySqlResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), resultRowEntry[3]?.ToString(), serverType);
            return resource;
        }
    }
}
