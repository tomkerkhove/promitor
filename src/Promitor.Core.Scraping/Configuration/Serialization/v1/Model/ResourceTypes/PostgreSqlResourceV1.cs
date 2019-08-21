namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class PostgreSqlResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The postgre server name.
        /// </summary>
        public string ServerName { get; set; }
    }
}
