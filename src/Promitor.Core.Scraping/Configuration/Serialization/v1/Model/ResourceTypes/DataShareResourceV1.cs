namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Data Share instance.
    /// </summary>
    public class DataShareResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The data share account name.
        /// </summary>
        public string AccountName { get; set; }
        
        /// <summary>
        /// The data share name.
        /// </summary>
        public string ShareName { get; set; }
    }
}
