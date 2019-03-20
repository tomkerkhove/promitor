using System;
using GuardNet;

namespace Promitor.Integrations.AzureStorage.Exceptions
{
    public class QueueNotFoundException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="queueName">Name of the queue that was not found</param>
        public QueueNotFoundException(string queueName) : base($"Azure Storage Queue '{queueName}' was not found")
        {
            Guard.NotNullOrEmpty(queueName, nameof(queueName));

            QueueName = queueName;
        }

        public string QueueName { get; }
    }
}