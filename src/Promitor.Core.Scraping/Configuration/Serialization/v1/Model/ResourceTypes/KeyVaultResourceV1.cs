namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Key Vault.
    /// </summary>
    public class KeyVaultResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Key Vault to get metrics for.
        /// </summary>
        public string VaultName { get; set; }
    }
}
