### Scraping agent starter configurations

This directory contains starter configuration file snippets for Promitor scraping agent to scrape metrics from the following Azure services.

| Azure service                    | File                  | Documentation |
|----------------------------------|-----------------------|---------------|
| Cosmos DB                        | `cosmosdb.yaml`       | [Microsoft.DocumentDB/databaseAccounts](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftdocumentdbdatabaseaccounts) |
| Key Vault                        | `keyvault.yaml`       | [Microsoft.KeyVault/vaults](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftkeyvaultvaults)  |
| Blob Storage                     | `storage-blob.yaml`   | [Microsoft.Storage/storageAccounts/blobServices](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftstoragestorageaccountsblobservices) |
| Virtual Machine Scale Set (VMSS) | `vmss.yaml`           | [Microsoft.Compute/virtualMachineScaleSets](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/metrics-supported#microsoftcomputevirtualmachinescalesets) |

