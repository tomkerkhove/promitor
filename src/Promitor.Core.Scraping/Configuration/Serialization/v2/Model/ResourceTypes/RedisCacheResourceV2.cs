namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class RedisCacheResourceV2 : AzureResourceDefinitionV2
    {
        /// <summary>
        /// The name of the redis cache.
        /// </summary>
        public string CacheName { get; set; }
    }
}
