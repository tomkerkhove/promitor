namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class KeyVaultResourceDefinition : AzureResourceDefinition
    {
        public KeyVaultResourceDefinition(string subscriptionId, string resourceGroupName, string vaultName)
          : base(ResourceType.KeyVault, subscriptionId, resourceGroupName)
        {
            VaultName = vaultName;
        }

        public string VaultName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => VaultName;
    }
}