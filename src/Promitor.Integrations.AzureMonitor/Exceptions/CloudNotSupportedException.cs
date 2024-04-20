using System;
using GuardNet;

namespace Promitor.Integrations.AzureMonitor.Exceptions
{
    public class CloudNotSupportedException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="cloud">Name of the azure cloud</param>
        public CloudNotSupportedException(string cloud) : base($"Configured Azure Monitor client cannot operate under '{cloud}'")
        {
            Guard.NotNullOrWhitespace(cloud, nameof(cloud));

            AzureCloud = cloud;
        }

        /// <summary>
        ///     The Azure Cloud(public, USGov, etc.)
        /// </summary>
        public string AzureCloud { get; }
    }
}