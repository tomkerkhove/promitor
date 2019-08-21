namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class CosmosDbResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The cosmos database name.
        /// </summary>
        public string DbName { get; set; }
    }
}
