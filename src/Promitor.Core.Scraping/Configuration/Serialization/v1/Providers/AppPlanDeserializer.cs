using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class AppPlanDeserializer : ResourceDeserializer
    {
        private const string AppPlanNameTag = "appPlanName";

        public AppPlanDeserializer(ILogger<AppPlanDeserializer> logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var appPlanName = node.GetString(AppPlanNameTag);

            return new AppPlanResourceV1
            {
               AppPlanName= appPlanName
            };
        }
    }
}
