
using Promitor.Core.Configuration;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Extensions
{
    public interface IAzureCloudEndpoints
    {
        AzureCloud Cloud { get; }
        AzureEndpoints Endpoints { get; }
    }
}