using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Promitor.Integrations.AzureStorageQueue
{
    public class AzureStorageQueueClient
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
            await queue.FetchAttributesAsync();
            var messageCount = queue.ApproximateMessageCount ?? 0;
            _logger.LogInformation("Current size of queue {0} is {1}", queueName, messageCount);
            return messageCount;
        }

        /// <summary>
        ///     Query Azure Storage to find out the duration of the recently inserted message
        /// </summary>
        /// <param name="accountName">Name of the account</param>
        /// <param name="queueName">Name of the Queue</param>
        /// <param name="sasToken">SAS token used to authenticate to Azure Storage</param>
        /// <returns>Duration of a recently inserted message in the queue</returns>
        public async Task<double> GetLastMessageDurationAsync(string accountName, string queueName, string sasToken)
        {
            var queue = GetQueueReference(accountName, queueName, sasToken);
            var msg = await queue.PeekMessageAsync();
            var duration = msg.InsertionTime != null ? DateTimeOffset.Now - msg.InsertionTime.Value : TimeSpan.Zero;
            _logger.LogInformation("Current duration of the last message in queue {0} is {1}", queueName, duration);
            return duration.TotalSeconds;
        }

        private static CloudQueue GetQueueReference(string accountName, string queueName, string sasToken)
        {
            
            var account = new CloudStorageAccount(new StorageCredentials(sasToken), accountName, null, true);
            var queueClient = account.CreateCloudQueueClient();
            return queueClient.GetQueueReference(queueName);
        }
    }
}