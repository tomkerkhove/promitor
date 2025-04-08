﻿using Promitor.Core.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Scraping.Configuration.Model
{
    public class AzureMetadata : IAzureCloudEndpoints
    {
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public AzureCloud Cloud { get; set; }
        public AzureEndpoints Endpoints { get; set; }
    }
}