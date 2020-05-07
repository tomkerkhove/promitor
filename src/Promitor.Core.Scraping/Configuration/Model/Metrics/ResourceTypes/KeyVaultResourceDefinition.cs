namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class KeyVaultResourceDefinition : AzureResourceDefinition
    {
        public KeyVaultResourceDefinition(string subscriptionId, string resourceGroupName, string keyVaultName)
          : base(ResourceType.KeyVault, subscriptionId, resourceGroupName)
        {
            KeyVaultName = keyVaultName;
        }

        public string KeyVaultName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => KeyVaultName;
    }
}