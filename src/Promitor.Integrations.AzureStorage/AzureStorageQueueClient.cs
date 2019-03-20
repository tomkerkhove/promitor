using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Promitor.Integrations.AzureStorage.Exceptions;

namespace Promitor.Integrations.AzureStorage
{
    public class AzureStorageQueueClient : AzureStorageClient
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="logger">Logger to use during interaction with Azure Storage</param>
        public AzureStorageQueueClient(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Queries Azure Storage to find out the message count of the queue
        /// </summary>
        /// <param name="accountName">Name of the account</param>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="sasToken">SAS token used to authenticate access to Azure Storage</param>
        /// <returns>Latest message count of the queue</returns>
        public async Task<int> GetQueueMessageCountAsync(string accountName, string queueName, string sasToken)
        {
            var queue = GetQueueReference(accountName, queueName, sasToken);
            var doesQueueExist = await queue.ExistsAsync();
            if (doesQueueExist == false)
            {
                throw new QueueNotFoundException(queueName);
            }

            await queue.FetchAttributesAsync();
            var messageCount = queue.ApproximateMessageCount ?? 0;
            _logger.LogInformation("Current size of queue {0} is {1}", queueName, messageCount);
            return messageCount;
        }

        private CloudQueue GetQueueReference(string accountName, string queueName, string sasToken)
        {
            var account = AuthenticateWithSasToken(accountName, sasToken);
            var queueClient = account.CreateCloudQueueClient();
            return queueClient.GetQueueReference(queueName);
        }
    }
}