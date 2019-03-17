﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Promitor.Integrations.AzureStorageQueue
{
    public class AzureStorageQueue
    {
        private readonly ILogger _logger;

        public AzureStorageQueue(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<int> GetQueueSizeAsync(string accountName, string queueName, string sasToken)
        {
            var account = new CloudStorageAccount(new StorageCredentials(sasToken), accountName, null, true);
            var queueClient = account.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            await queue.FetchAttributesAsync();
            var queueSize = queue.ApproximateMessageCount ?? 0;
            _logger.LogInformation("Current size of queue {0} is {1}", queueName, queueSize);
            return queueSize;
        }

        public async Task<double> GetLastMessageDurationAsync(string accountName, string queueName, string sasToken)
        {
            var account = new CloudStorageAccount(new StorageCredentials(sasToken), accountName, null, true);
            var queueClient = account.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            var msg = await queue.PeekMessageAsync();
            var duration = msg.InsertionTime != null ? DateTimeOffset.Now - msg.InsertionTime.Value : TimeSpan.Zero;
            _logger.LogInformation("Current duration of the last message in queue {0} is {1}", queueName, duration);
            return duration.TotalSeconds;
        }
    }
}