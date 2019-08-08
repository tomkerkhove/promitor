namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class CosmosDbResourceV2 : AzureResourceDefinitionV2
    {
        /// <summary>
        /// The cosmos database name.
        /// </summary>
        public string DbName { get; set; }
    }
}
