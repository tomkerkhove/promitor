namespace Promitor.Integrations.AzureMonitor
{
    /// <summary>
    /// Metric type in Azure
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/azure/storage/common/storage-metrics-in-azure-monitor
    /// </remarks>
    public enum MetricType
    {
        NotSpecified = 0,
        Capacity = 1,
        Transactional = 2
    }
}