namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class PostgreSqlResourceV2 : AzureResourceDefinitionV2
    {
        /// <summary>
        /// The postgre server name.
        /// </summary>
        public string ServerName { get; set; }
    }
}
