namespace Promitor.Core.Configuration
{
    public class AzureEndpoints
    {
        public string AuthenticationEndpoint { get; set; }
        public string ResourceManagerEndpoint { get; set; }
        public string GraphEndpoint { get; set; }
        public string ManagementEndpoint { get; set; }
        public string StorageEndpointSuffix { get; set; }
        public string KeyVaultSuffix { get; set; }
        public string MetricsQueryAudience { get; set; }
        public string MetricsClientAudience { get; set; }
    }
}
