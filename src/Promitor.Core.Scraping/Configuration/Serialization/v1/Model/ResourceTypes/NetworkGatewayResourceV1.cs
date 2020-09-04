﻿namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Network Gateway.
    /// </summary>
    public class NetworkGatewayResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Network Gateway to get metrics for.
        /// </summary>
        public string NetworkGatewayName { get; set; }
    }
}
