namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Represents an Azure resource that can be scraped.
    /// </summary>
    public class AzureResourceDefinitionV1
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
    }
}
