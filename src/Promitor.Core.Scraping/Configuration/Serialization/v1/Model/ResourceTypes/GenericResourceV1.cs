namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class GenericResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The filter for the Azure metric query.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// The URI for the resource to get metrics for.
        /// </summary>
        public string ResourceUri { get; set; }
    }
}
