using GuardNet;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Promitor.Integrations.AzureStorage
{
    public class AzureStorageClient
    {
        /// <summary>
        ///     Authenticate with Azure Storage
        /// </summary>
        /// <param name="accountName">Name of the storage account</param>
        /// <param name="sasToken">SAS token to authenticate with</param>
        /// <returns>Authenticated storage account</returns>
        protected CloudStorageAccount AuthenticateWithSasToken(string accountName, string sasToken)
        {
            Guard.NotNullOrEmpty(accountName, nameof(accountName));
            Guard.NotNullOrEmpty(sasToken, nameof(sasToken));
            
            var storageCredentials = new StorageCredentials(sasToken);

            return new CloudStorageAccount(storageCredentials, accountName, endpointSuffix: null, useHttps: true);
        }
    }
}