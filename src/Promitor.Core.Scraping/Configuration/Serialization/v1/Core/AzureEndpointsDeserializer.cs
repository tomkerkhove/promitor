using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureEndpointsDeserializer : Deserializer<AzureEndpointsV1>
    {
        public AzureEndpointsDeserializer(ILogger<AzureEndpointsDeserializer> logger) : base(logger)
        {
            Map(endpoints => endpoints.AuthenticationEndpoint)
                .IsRequired()
                .MapUsing(ValidateUrl);
            Map(endpoints => endpoints.ResourceManagerEndpoint)
                .IsRequired()
                .MapUsing(ValidateUrl);
            Map(endpoints => endpoints.GraphEndpoint)
                .IsRequired()
                .MapUsing(ValidateUrl);
            Map(endpoints => endpoints.ManagementEndpoint)
                .IsRequired()
                .MapUsing(ValidateUrl);
            Map(endpoints => endpoints.StorageEndpointSuffix)
                .IsRequired();
            Map(endpoints => endpoints.KeyVaultSuffix)
                .IsRequired();
            Map(endpoints => endpoints.MetricsQueryAudience)
                .MapUsing(ValidateUrl); // Should this be required?
            Map(endpoints => endpoints.MetricsClientAudience)
                .MapUsing(ValidateUrl); // Should this be required?
        }

        private object ValidateUrl(string url, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                errorReporter.ReportError(nodePair.Value, $"'{url}' is not a valid URL.");
                return null;
            }

            return url;
        }
    }
}
