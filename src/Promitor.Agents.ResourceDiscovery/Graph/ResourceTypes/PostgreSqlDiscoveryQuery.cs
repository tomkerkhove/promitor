using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class PostgreSqlDiscoveryQuery : ResourceDiscoveryQuery
    {
        private const string SingleServerResourceType = "microsoft.dbforpostgresql/servers";
        private const string FlexibleServerResourceType = "microsoft.dbforpostgresql/flexibleservers";
        private const string HyperscaleServerResourceType = "microsoft.dbforpostgresql/servergroupsv2";

        public override string[] ResourceTypes => new[] { SingleServerResourceType, FlexibleServerResourceType, HyperscaleServerResourceType };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "type", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            PostgreSqlServerType serverType = PostgreSqlServerType.Single;

            var resourceType = resultRowEntry[2]?.ToString();
            switch (resourceType?.ToLower())
            {
                case SingleServerResourceType:
                    serverType = PostgreSqlServerType.Single;
                    break;
                case FlexibleServerResourceType:
                    serverType = PostgreSqlServerType.Flexible;
                    break;
                case HyperscaleServerResourceType:
                    serverType = PostgreSqlServerType.Hyperscale;
                    break;
            }

            var resource = new PostgreSqlResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), resultRowEntry[3]?.ToString(), serverType);
            return resource;
        }
    }
}
