namespace Promitor.Core.Contracts.ResourceTypes
{
    public class KeyVaultResourceDefinition : AzureResourceDefinition
    {
        public KeyVaultResourceDefinition(string subscriptionId, string resourceGroupName, string vaultName)
          : base(ResourceType.KeyVault, subscriptionId, resourceGroupName, vaultName)
        {
            VaultName = vaultName;
        }

        public string VaultName { get; }
    }
}