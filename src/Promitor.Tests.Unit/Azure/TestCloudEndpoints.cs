
using Promitor.Core.Configuration;
using Promitor.Core.Serialization.Enum;
using Promitor.Core.Extensions;

namespace Promitor.Tests.Unit.Azure
{
    internal class TestCloudEndpoints : IAzureCloudEndpoints
    {
        public TestCloudEndpoints(AzureCloud cloud, AzureEndpoints endpoints)
        {
            Cloud = cloud;
            Endpoints = endpoints;
        }

        public AzureCloud Cloud { get; }
        public AzureEndpoints Endpoints { get; }
    }
}